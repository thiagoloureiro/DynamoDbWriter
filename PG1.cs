using Amazon.DynamoDBv2.DataModel;
using System.IO;

namespace DynamoDbWriter
{
    [DynamoDBTable("PG1")]
    public class PG1
    {
        [DynamoDBHashKey]
        public string ProcessGroupId { get; set; }

        [DynamoDBRangeKey]
        public byte[] physicalpp_timerange { get; set; }

        [DynamoDBProperty("Payload")]
        public string Payload { get; set; }
    }
}