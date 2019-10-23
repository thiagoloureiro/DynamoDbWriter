using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbWriter
{
    public class DbContextFactory
    {
        public DynamoDBContext CreateDbContext()
        {
            return new DynamoDBContext(AmazonDynamoDb.GetClient());
        }

        public AmazonDynamoDBClient GetDynamoDbClient()
        {
            return AmazonDynamoDb.GetClient();
        }
    }
}