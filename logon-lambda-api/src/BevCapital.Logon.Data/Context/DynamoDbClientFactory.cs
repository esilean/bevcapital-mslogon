using Amazon;
using Amazon.DynamoDBv2;

namespace BevCapital.Logon.Data.Context
{
    public static class DynamoDbClientFactory
    {
        public static AmazonDynamoDBClient CreateClient()
        {
            var dynamoDbConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.SAEast1
            };
            return new AmazonDynamoDBClient(dynamoDbConfig);
        }
    }
}
