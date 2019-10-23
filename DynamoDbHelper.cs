using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDbWriter
{
    public class DynamoDbHelper : IDynamoDbHelper
    {
        public async Task<IList<T>> GetAll<T>()
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                return await context.ScanAsync<T>(new List<ScanCondition>()).GetRemainingAsync();
            }
        }

        public async Task CreateTable(string tableName, string hashKey, ScalarAttributeType hashKeyType, string rangeKey = null, ScalarAttributeType rangeKeyType = null)
        {
            var schemaElements = new List<KeySchemaElement>();
            var attributeDefinitions = new List<AttributeDefinition>();

            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = tableName,
                BillingMode = BillingMode.PAY_PER_REQUEST
                //ProvisionedThroughput = new ProvisionedThroughput
                //{
                //    ReadCapacityUnits = 20000,
                //    WriteCapacityUnits = 20000
                //}
            };

            schemaElements.Add(new KeySchemaElement
            {
                AttributeName = hashKey,
                KeyType = KeyType.HASH
            });

            attributeDefinitions.Add(new AttributeDefinition
            {
                AttributeName = hashKey,
                AttributeType = hashKeyType
            }
            );

            if (!string.IsNullOrEmpty(rangeKey) && !string.IsNullOrEmpty(rangeKeyType))
            {
                schemaElements.Add(new KeySchemaElement
                {
                    AttributeName = rangeKey,
                    KeyType = KeyType.RANGE
                });
                attributeDefinitions.Add(new AttributeDefinition
                {
                    AttributeName = rangeKey,
                    AttributeType = rangeKeyType
                }
               );
            }

            createRequest.KeySchema.AddRange(schemaElements);
            createRequest.AttributeDefinitions.AddRange(attributeDefinitions);

            try
            {
                var client = new DbContextFactory().GetDynamoDbClient();
                await client.CreateTableAsync(createRequest);
                bool isTableAvailable = false;
                while (!isTableAvailable)
                {
                    Thread.Sleep(2000);
                    var tableStatus = await client.DescribeTableAsync(tableName);
                    isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: failed to create the new table: {ex.Message}");
            }
        }

        public async Task<IList<T>> GetRows<T>(object keyValue, List<ScanCondition> scanConditions = null)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                DynamoDBOperationConfig config = null;

                if (scanConditions != null && scanConditions.Count > 0)
                {
                    config = new DynamoDBOperationConfig()
                    {
                        QueryFilter = scanConditions
                    };
                }

                return await context.QueryAsync<T>(keyValue, config).GetRemainingAsync().ConfigureAwait(false);
            }
        }

        public async Task<IList<T>> GetRows<T>(List<ScanCondition> scanConditions)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                return await context.ScanAsync<T>(scanConditions).GetRemainingAsync().ConfigureAwait(false);
            }
        }

        public async Task<T> Load<T>(object keyValue)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                return await context.LoadAsync<T>(keyValue).ConfigureAwait(false);
            }
        }

        public async Task Save<T>(T document)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                await context.SaveAsync(document).ConfigureAwait(false);
            }
        }

        public async Task Delete<T>(T document)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                await context.DeleteAsync(document).ConfigureAwait(false);
            }
        }

        public async Task BatchSave<T>(IEnumerable<T> documents)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                var batch = context.CreateBatchWrite<T>();
                batch.AddPutItems(documents);
                await batch.ExecuteAsync().ConfigureAwait(false);
            }
        }

        public async Task BatchDelete<T>(IEnumerable<T> documents)
        {
            using (var context = new DbContextFactory().CreateDbContext())
            {
                var batch = context.CreateBatchWrite<T>();
                batch.AddDeleteItems(documents);
                await batch.ExecuteAsync().ConfigureAwait(false);
            }
        }
    }
}