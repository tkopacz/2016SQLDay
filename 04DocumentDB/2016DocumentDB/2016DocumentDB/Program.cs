using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2016DocumentDB
{
    public class Customer
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int MonthBirthday { get; set; }
    }

    public class Customers
    {
        public Customer[] Arr { get; set; }
    }
    class Program
    {
        private static readonly string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseName = ConfigurationManager.AppSettings["DatabaseId"];
        private static readonly string collectionName = ConfigurationManager.AppSettings["CollectionId"];
        private static readonly ConnectionPolicy connectionPolicy = new ConnectionPolicy { UserAgentSuffix = " samples-net/3" };

        private static DocumentClient client;

        static void Main(string[] args)
        {
            using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey))
            {
                //Connect to existing collection
                CreateDocumentsAsync().Wait();
            }
        }

        //SELECT * FROM root c where c.type="company"
        //SELECT * FROM root c where c.type="companyv2"
        private static async Task CreateDocumentsAsync()
        {
            Debugger.Break();
            //Delete old documents (better - delete collection)1
            Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);

            var docs = client.CreateDocumentQuery(collectionLink);
            foreach (var doc in docs) await client.DeleteDocumentAsync(doc.SelfLink);


            Debugger.Break();
            // Create a dynamic object
            dynamic customer1 = new
            {
                id = "1",
                Name = "Tomasz",
                Surname = "Kopacz",
                MonthBirthday = 5
            };
            ResourceResponse<Document> response = await client.CreateDocumentAsync(collectionLink, customer1);
            var createdDocument = response.Resource;

            dynamic customer2 = new
            {
                id = "2",
                Name = "Jan",
                Surname = "Kopacz",
                MonthBirthday = 5,
                VIP = true
            };
            response = await client.CreateDocumentAsync(collectionLink, customer2);
            createdDocument = response.Resource;
            
            Console.WriteLine("Document with id {0} created", createdDocument.Id);
            Console.WriteLine("RU: {0}", response.RequestCharge);
            
            //Create a strongly-typed object
            Customer c1 = new Customer() { id="3", Name = "Adam", Surname = "Nowak", MonthBirthday = 1 };
            response = await client.CreateDocumentAsync(collectionLink, c1);
            createdDocument = response.Resource;

            //Create document with multiple customers!
            Customer[] cArr = new Customer[] 
                {
                    new Customer() { id = "4", Name = "Adam1", Surname = "Nowak", MonthBirthday = 1 },
                    new Customer() { id = "5", Name = "Adam2", Surname = "Nowak", MonthBirthday = 5 }
                };
            Customers cs = new Customers();
            cs.Arr = cArr;
            response = await client.CreateDocumentAsync(collectionLink, cs);
            Console.WriteLine("RU: {0}", response.RequestCharge);
            createdDocument = response.Resource;

            //Kwerendy
            var result = client.CreateDocumentQuery(collectionLink, "select * from root r where r.MonthBirthday = 5").AsEnumerable();
            Console.WriteLine($"{result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine(item); //Why only 2?
            }
            
            dynamic customer6 = new
            {
                id = "6",
                Name = "Piotr",
                Surname = "Kopacz",
                MonthBirthday = 5,
                VIP = true
            };

            response = await client.CreateDocumentAsync(collectionLink, customer6, new RequestOptions() {
                IndexingDirective = IndexingDirective.Exclude
            });
            Console.WriteLine("RU: {0}", response.RequestCharge);
            result = client.CreateDocumentQuery<Customer>(collectionLink).Where(f=>f.MonthBirthday>=5).AsEnumerable();
            Console.WriteLine($"{result.Count()}"); //Still 2 !
            //Mention: EnableScanInQuery

            //"Update"
            result = client.CreateDocumentQuery<Customer>(collectionLink).Where(f => f.Name == "Tomasz" && f.Surname == "Kopacz");
            var tk = (Customer)result.FirstOrDefault();
            Document dtk = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, tk.id));
            Console.WriteLine(dtk);
            dtk.SetPropertyValue("VIP", true);
            //Replace
            await client.ReplaceDocumentAsync(dtk); //change ETAG
            try
            {
                dtk.SetPropertyValue("Something", 123);
                //Insert or update
                response = await client.UpsertDocumentAsync(dtk.SelfLink, dtk, new RequestOptions()
                {
                    AccessCondition = new AccessCondition { Condition = dtk.ETag /* old ETAG*/, Type = AccessConditionType.IfMatch }
                }
                );
            } catch (DocumentClientException ex)
            {
                Console.WriteLine($"Conflict: {ex.ToString()}");
            }
        }
    }
}
