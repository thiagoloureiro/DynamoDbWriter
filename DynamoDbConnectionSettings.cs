using Amazon;

namespace DynamoDbWriter
{
    public class DynamoDbConnectionSettings
    {
        /// <summary>
        /// AWS Access Key
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// AWS Secret Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// ap-southeast-2
        /// Example: dynamodb.ap-southeast-2.amazonaws.com
        /// https://docs.aws.amazon.com/general/latest/gr/rande.html#ddb_region
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// AWS RegionEndPoint, example: RegionEndpoint.APSoutheast2
        /// </summary>
        public RegionEndpoint RegionEndPoint { get; set; }

        /// <summary>
        /// Client Timeout, in seconds
        /// </summary>
        public int Timeout { get; set; } = 100; // Default value is 100 seconds

        /// <summary>
        /// Maximum retry
        /// </summary>
        public int MaxErrorRetry { get; set; } = 4; // Default value is 4 retries

        /// <summary>
        /// Disable Logging
        /// </summary>
        public bool DisableLogging { get; set; } = true;
    }
}