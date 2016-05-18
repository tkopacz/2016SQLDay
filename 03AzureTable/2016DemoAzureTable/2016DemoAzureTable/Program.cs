//#define DELETE
//#define ZEROCOUNT
//#define CREATE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Azure Storage:
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Threading;
using System.Diagnostics;

/*
 * Remarks - better is to use ASYNC version of API - but harder to debug / demo
 */

namespace _2016DemoAzureTable
{

    /// <summary>
    /// Logging - demo
    /// </summary>
    public class LogEntity : TableEntity
    {
        public LogEntity(string serverName, DateTime dt)
        {
            //PK,RK: up to 1KB
            this.PartitionKey = serverName;
            //Guid.NewGuid() - just in case - "unique" id
            this.RowKey = $"{dt:yyyyMMdd}_{dt.Ticks}_{Guid.NewGuid().ToString("N")}";
        }

        public LogEntity() { }

        /// <summary>
        /// Server
        /// Good candidate for Partition Key!!!
        /// </summary>
        [IgnorePropertyAttribute]
        public string ServerName { get { return this.PartitionKey; } }

        /// <summary>
        /// Date of event
        /// Day - is good candidate for Row Key
        /// </summary>
        public DateTime Dt { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public int Severity { get; set; }
    }

    public class LogCountEntity : TableEntity
    {
        public LogCountEntity() { }
        public LogCountEntity(string serverName, int severity)
        {
            this.PartitionKey = $"{serverName}_{severity}";
            this.RowKey = "1";// Single row only!    
        }

        public int Cnt { get; set; }

    }

    /// <summary>
    /// Simple politic 
    /// </summary>
    public class MyAlwaysRetryPolicy : IRetryPolicy
    {
        public IRetryPolicy CreateInstance()
        {
            return new MyAlwaysRetryPolicy();
        }

        public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext)
        {
            //Thread.Sleep(5000); NIE - śpi się na zewnątrz tego kodu - to tylko mówi - jak długo spać 
            retryInterval = TimeSpan.FromSeconds(5);
            Console.WriteLine("RETRY:" + lastException.Message);
            return true; //Wisi
        }
    }

    public static class Util
    {
        public static async Task Retry(Func<Task> t)
        {
            int cnt = 20;
            while (--cnt > 0)
            {
                try
                {
                    await t();
                    break;

                }
                catch (StorageException ex) when (cnt > 0)
                {
                    Console.WriteLine("Auć!");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serverNames = new string[] { "server1", "server2", "server3", "octopus", "marysia" };
            var messageCodes = new string[] { "CODE", "ABC", "DEF", "GHA", "UBQ" };

            //Get storage connection pltkw3st20160501
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            //Get table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Scenario 1 - log with many entities
            //Naming: "^[A-Za-z][A-Za-z0-9]{2,62}$".
            CloudTable tableLog2016 = tableClient.GetTableReference("log2016");
            CloudTable tableLogCount2016 = tableClient.GetTableReference("logcount2016");
            TableOperation tblOper;

#if DELETE
            tableLog2016.DeleteIfExists();
            tableLogCount2016.DeleteIfExists();
            var ct = new CancellationTokenSource();
            Task.Run(async () =>
            {
                //Because DeleteIfExists need to remove all entities!
                await tableLog2016.CreateIfNotExistsAsync(new TableRequestOptions() { RetryPolicy = new MyAlwaysRetryPolicy() }, null, ct.Token);
                await tableLogCount2016.CreateIfNotExistsAsync(new TableRequestOptions() { RetryPolicy = new MyAlwaysRetryPolicy() }, null, ct.Token);
            }).Wait();
#endif
#if ZEROCOUNT
            for (int j = 0; j < serverNames.Count(); j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    LogCountEntity lc = new LogCountEntity(serverNames[j], i);
                    tblOper = TableOperation.InsertOrReplace(lc);
                    tableLogCount2016.Execute(tblOper);
                }
            }
#endif
#if CREATE
            //Remarks:
            //What if we have no PK candidate?
            //Use fixed number of partition and - select one randomly
            //Init LogCountEntity


            //var dtStart = new DateTime(2016, 01, 01); //TK: Was from 2016
            var dtStart = new DateTime(2015, 01, 01); //TK: Was from 2016
            Task.Run(async () =>
            {
                try
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        //DateTime dt = dtStart.AddDays(-i).AddSeconds(DateTime.Now.Second);
                        DateTime dt = dtStart.AddMinutes(-i).AddSeconds(DateTime.Now.Second);
                        LogEntity le = new LogEntity(
                            serverNames[i % serverNames.Count()],
                            dt
                            );
                        le.MessageCode = messageCodes[i % messageCodes.Count()] + (i % 7).ToString();
                        le.Message = $"Message{i}";
                        le.Dt = dt;
                        le.Severity = i % 6;
                        tblOper = TableOperation.InsertOrReplace(le);
                        await tableLog2016.ExecuteAsync(tblOper);
                        //Update count
                        await Util.Retry(async () =>
                           {
                               LogCountEntity lc;
                               var tblOperLc = TableOperation.Retrieve<LogCountEntity>($"{le.ServerName}_{le.Severity}", "1");
                               lc = (LogCountEntity)tableLogCount2016.Execute(tblOperLc).Result;
                               lc.Cnt++;
                               tblOperLc = TableOperation.Replace(lc);
                               await tableLogCount2016.ExecuteAsync(tblOperLc);
                           });
                        if (i % 1000 == 0) Console.WriteLine($"{i}");
                    }
                } catch (Exception ex)
                {
                    throw;
                }
            }).Wait();
#endif
            //pltkw3st20160501
            //Scenario 1 - how many Logs with Severity 3?
            Debugger.Break();
            int totalCount = 0;
            Parallel.For(0, serverNames.Count(),(i) =>
                {
                    tblOper = TableOperation.Retrieve<LogCountEntity>($"{serverNames[i]}_3", "1");
                    LogCountEntity lc = (LogCountEntity)tableLogCount2016.Execute(tblOper).Result;
                    totalCount += lc.Cnt;
                }
            );
            Debugger.Break();
            Console.WriteLine($"1, TPL, Severity 3: {totalCount}");

            //Scenario 2 - logs from server marysia from 2015-12-03 to 2015-12-20
            string filter = 
                TableQuery.CombineFilters(
                  TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "marysia"),
                    TableOperators.And,
                  TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "20151203"),
                      TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "20151220")
                )
             );
            Console.WriteLine($"2: Partition + rowkey range, ok: {filter}");
            Debugger.Break();
            TableQuery<LogEntity> query = new TableQuery<LogEntity>().Where(filter);
            foreach (var item in tableLog2016.ExecuteQuery<LogEntity>(query))
            {
                Console.WriteLine($"{item.Dt}, {item.MessageCode}");
            }
            Debugger.Break();
            //Scenario 2a - logs from server marysia from 2015-12-03 00:00:26 to 2015-12-20
            filter =
                TableQuery.CombineFilters(
                    TableQuery.CombineFilters(
                      TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "marysia"),
                        TableOperators.And,
                      TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "20151203"),
                          TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "20151220")
                  )),
                    TableOperators.And,
                  TableQuery.GenerateFilterConditionForDate("Dt", QueryComparisons.GreaterThan, new DateTimeOffset(2015, 12, 03, 00, 00, 26,TimeSpan.Zero))
             );
            Console.WriteLine($"2a: Partition + rowkey range, ok: {filter}");
            Debugger.Break();
            query = new TableQuery<LogEntity>().Where(filter);
            foreach (var item in tableLog2016.ExecuteQuery<LogEntity>(query))
            {
                Console.WriteLine($"{item.Dt}, {item.MessageCode}");
            }
            Debugger.Break();
            //Scenario 3 - logs with code UBQ0
            filter = TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "20151203"),
                          TableOperators.And,
                        TableQuery.GenerateFilterCondition("MessageCode", QueryComparisons.Equal, "UBQ0")
                      );
            //TableScan!
            Console.WriteLine($"3: Table Scan!! {filter}");
            query = new TableQuery<LogEntity>().Where(filter);
            /* https://msdn.microsoft.com/en-us/library/azure/dd179421.aspx - Query Entities
             * Continuation Token:
             *     When the number of entities to be returned exceeds 1,000.
             *     When the server timeout interval is exceeded.
             *     When a server boundary is reached, if the query returns data that is spread across multiple servers.
             *     5 second
             *     https://msdn.microsoft.com/en-us/library/azure/dd135718.aspx 
             *     30 second for total server processing
            */
            var result = tableLog2016.ExecuteQuery<LogEntity>(query);
            totalCount=result.Count(); //Calculated on CLIENT SIDE!
            Console.WriteLine($"{totalCount}");
            Debugger.Break();
            //Scenario 4 - Customers, Queue, ...
            CustomerProcess cp = new CustomerProcess();
            cp.Demo(storageAccount);
            Debugger.Break();
            Console.WriteLine("Enter - close queue processing!");
            Console.ReadLine();
        }
    }
}
