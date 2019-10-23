using Amazon.DynamoDBv2;
using Amazon.Runtime;
using System;

namespace DynamoDbWriter
{
    public class AmazonDynamoDb
    {
        private static AmazonDynamoDBClient _client;

        private static DynamoDbConnectionSettings _dynamoDbConnectionSettings;

        public AmazonDynamoDb(DynamoDbConnectionSettings dynamoDbConnectionSettings)
        {
            _dynamoDbConnectionSettings = dynamoDbConnectionSettings;
        }

        public static AmazonDynamoDBClient GetClient()
        {
            if (_client != null) { return _client; }

            try
            {
                var clientConfig = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = _dynamoDbConnectionSettings.RegionEndPoint,
                    Timeout = TimeSpan.FromSeconds(_dynamoDbConnectionSettings.Timeout),
                    MaxErrorRetry = _dynamoDbConnectionSettings.MaxErrorRetry,
                    DisableLogging = _dynamoDbConnectionSettings.DisableLogging
                };

                if (_dynamoDbConnectionSettings.ServiceUrl != null) clientConfig.ServiceURL = _dynamoDbConnectionSettings.ServiceUrl;

                _client = new AmazonDynamoDBClient(_dynamoDbConnectionSettings.AccessKeyId, _dynamoDbConnectionSettings.SecretKey, clientConfig);
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine($"Error (AmazonDynamoDBException) creating DynamoDb client", ex);
            }
            catch (AmazonServiceException ex)
            {
                Console.WriteLine($"Error (AmazonServiceException) creating DynamoDb client", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating DynamoDb client", ex);
            }

            return _client;
        }
    }
}