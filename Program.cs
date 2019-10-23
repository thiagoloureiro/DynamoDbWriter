using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DynamoDbWriter
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IDynamoDbHelper dbHelper = new DynamoDbHelper();
            var searchGuid = Guid.NewGuid();

            var dynamoDbConnectionSettings = new DynamoDbConnectionSettings
            {
                AccessKeyId = "-",
                DisableLogging = true,
                MaxErrorRetry = 10,
                SecretKey = "-",
                Timeout = 5000,
                //RegionEndPoint = RegionEndpoint.APSoutheast2,
                ServiceUrl = "http://localhost:8000"
            };

            var dynamoDb = new AmazonDynamoDb(dynamoDbConnectionSettings);
            string json;

            using (var r = new StreamReader("content.json"))
            {
                json = r.ReadToEnd();
            }

            Console.WriteLine("Creating DynamoDb Table");

            await CreateTable(dbHelper, 1);

            Console.WriteLine("Saving Data to DynamoDb Table");

            #region Scenario 01

            // Scenario 01:
            Console.WriteLine("Scenario 01");
            Console.WriteLine("Writing 10k single");
            var sw1 = new Stopwatch();
            sw1.Start();

            for (int i = 0; i < 10000; i++)
            {
                await SaveData(dbHelper, searchGuid, json);
            }

            Console.WriteLine(sw1.Elapsed);

            #endregion Scenario 01

            #region Scenario 02

            // Scenario 02:
            Console.WriteLine("Scenario 02");
            Console.WriteLine("Writing 10k single Batch");

            var lstPG = new List<PG1>();

            for (int i = 0; i < 100; i++)
            {
                var binaryData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

                lstPG.Add(new PG1
                {
                    PG_ID = Guid.NewGuid().ToString(),
                    Payload = json,
                    PP_TS = binaryData
                });
            }

            var lst = new List<Task>();

            var sw2 = new Stopwatch();

            for (int i = 0; i < 100; i++)
            {
                sw2.Start();
                Task t = Task.Run(() => dbHelper.BatchSave(lstPG));

                lst.Add(t);
            }

            await Task.WhenAll(lst);

            Console.WriteLine(sw2.Elapsed);

            #endregion Scenario 02

            #region Scenario 03

            // Scenario 03:
            Console.WriteLine("Scenario 03");
            Console.WriteLine("Writing 10k in 100 Parallel Batches (100 each)");

            var lstPG3 = new List<PG1>();

            for (int i = 0; i < 100; i++)
            {
                var binaryData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

                lstPG3.Add(new PG1
                {
                    PG_ID = Guid.NewGuid().ToString(),
                    Payload = json,
                    PP_TS = binaryData
                });
            }

            var lstTsk = new List<Task>();

            var sw3 = new Stopwatch();

            for (int i = 0; i < 100; i++)
            {
                sw3.Start();
                Task t = Task.Run(() => dbHelper.BatchSave(lstPG3));

                lstTsk.Add(t);
            }

            await Task.WhenAll(lstTsk);

            Console.WriteLine(sw3.Elapsed);

            #endregion Scenario 03
        }

        private static async Task SaveData(IDynamoDbHelper dbHelper, Guid searchGuid, string json)
        {
            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            await dbHelper.Save(new PG1
            {
                PG_ID = Guid.NewGuid().ToString(),
                Payload = json,
                PP_TS = sevenItems
            });
        }

        private static async Task CreateTable(IDynamoDbHelper dbHelper, int i)
        {
            await dbHelper.CreateTable($"PG{i}", "PG_ID", ScalarAttributeType.S, "PP_TS",
                ScalarAttributeType.B);
        }
    }
}