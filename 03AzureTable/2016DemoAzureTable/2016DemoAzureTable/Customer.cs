using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;
using System.Threading;

namespace _2016DemoAzureTable
{
    /// <summary>
    /// Message for Azure Queue
    /// </summary>
    public class Customer
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int MonthBirthday { get; set; }
    }

    /// <summary>
    /// Base customer entity
    /// </summary>
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity() { }
        public CustomerEntity(string name, string surname, int monthbirthday)
        {
            this.PartitionKey = surname;
            this.RowKey = $"{name}_{Guid.NewGuid().ToString("N")}";
            this.Name = name;
            this.MonthBirthday = monthbirthday;
        }
        [IgnorePropertyAttribute]
        public string Surname { get { return this.PartitionKey; } }
        public string Name { get; set; } //Or extrac from rowkey
        public int MonthBirthday { get; set; }
    }

    /// <summary>
    /// To find all customer with particular name
    /// </summary>
    public class NameToCustomerEntity : TableEntity
    {
        public NameToCustomerEntity() { }
        public NameToCustomerEntity(string name, string surname, int monthbirthday, string customerEntityRK)
        {
            this.PartitionKey = name;
            this.RowKey = $"{surname}_{Guid.NewGuid().ToString("N")}";
            this.Surname = surname;
            this.MonthBirthday = monthbirthday;
            this.CustomerEntityRK = customerEntityRK;
        }

        public string CustomerEntityRK { get; set; }
        public int MonthBirthday { get; set; }
        public string Surname { get; set; }
    }

    /// <summary>
    /// To find all customer with birhday in particular month
    /// </summary>
    public class MonthBirthdayToCustomerEntity : TableEntity
    {
        public MonthBirthdayToCustomerEntity() { }
        public MonthBirthdayToCustomerEntity(string name, string surname, int monthbirthday, string customerEntityRK)
        {
            this.PartitionKey = monthbirthday.ToString();
            this.RowKey = $"{surname}_{name}_{Guid.NewGuid().ToString("N")}";
            this.Surname = surname;
            this.MonthBirthday = monthbirthday;
            this.CustomerEntityRK = customerEntityRK;
        }

        public string CustomerEntityRK { get; set; }
        public int MonthBirthday { get; set; }
        public string Surname { get; set; }
    }

    public class CustomerProcess
    {
        public void Demo(CloudStorageAccount storageAccount)
        {
            Debugger.Break();
            var q = storageAccount.CreateCloudQueueClient().GetQueueReference("queueadd");
            q.CreateIfNotExists();
            Task.Run(() => ProcessQueue(storageAccount)); //Start "background" processing
            var ts = DateTime.Now.Ticks;
            for (long i = ts; i < ts + 100; i++)
            {
                Customer c = new Customer();
                c.MonthBirthday = (int)(1 + i % 13);
                c.Name = $"Name{(i % 5):D10}";
                c.Surname = $"Surname{(i % 11):D10}";
                q.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(c)));
                Thread.Sleep(2000);
            }
        }

        public async Task ProcessQueue(CloudStorageAccount storageAccount)
        {
            storageAccount.CreateCloudQueueClient();
            var q = storageAccount.CreateCloudQueueClient().GetQueueReference("queueadd");
            var tce = storageAccount.CreateCloudTableClient().GetTableReference("customer");
            var tnce = storageAccount.CreateCloudTableClient().GetTableReference("nametocustomerentity");
            var tmce = storageAccount.CreateCloudTableClient().GetTableReference("monthbirthdaytocustomerentity");
            tce.CreateIfNotExists();
            tnce.CreateIfNotExists();
            tmce.CreateIfNotExists();
            while (true)
            {
                var msg = q.GetMessage();
                if (msg != null)
                {
                    try
                    {
                        Customer c = JsonConvert.DeserializeObject<Customer>(msg.AsString);
                        CustomerEntity ce = new CustomerEntity(c.Name, c.Surname, c.MonthBirthday);
                        TableOperation tblOper = TableOperation.InsertOrReplace(ce);
                        await tce.ExecuteAsync(tblOper);
                        if (msg.DequeueCount > 1)
                        {
                            Console.WriteLine($"RETRY - {msg.Id}");
                            //We already processed this message. Additional logic etc.
                        }
                        else
                        {
                            //Failure demo :)
                            if (c.MonthBirthday > 3) throw new StorageException();
                        }
                        NameToCustomerEntity nce = new NameToCustomerEntity(c.Name, c.Surname, c.MonthBirthday, ce.RowKey);
                        tblOper = TableOperation.InsertOrReplace(nce);
                        await tnce.ExecuteAsync(tblOper);
                        MonthBirthdayToCustomerEntity mce = new MonthBirthdayToCustomerEntity(c.Name, c.Surname, c.MonthBirthday, ce.RowKey);
                        tblOper = TableOperation.InsertOrReplace(mce);
                        await tmce.ExecuteAsync(tblOper);

                        await q.DeleteMessageAsync(msg); //Processed correctly
                        Console.WriteLine($"OK - {msg.Id}");
                    }
                    catch (StorageException ex)
                    {
                        Console.WriteLine($"StorageException - {msg.Id}");
                        //Retry
                    }
                }
                else
                {
                    await Task.Delay(5000);
                    Console.WriteLine("Waiting for message");
                }
            }
        }
    }
}
