using Amazon.DynamoDBv2.DataModel;
using System.IO;

namespace DynamoDbWriter
{
    [DynamoDBTable("PG1")]
    public class PG1
    {
        [DynamoDBHashKey]
        public string PG_ID { get; set; }

        [DynamoDBRangeKey]
        public byte[] PP_TS { get; set; }

        [DynamoDBProperty("Payload")]
        public string Payload { get; set; }
    }
}