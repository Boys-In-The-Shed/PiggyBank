using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiggyBank.Database
{
    public class DynamoKeyValueStore : IKeyValueStore
    {
        private readonly string _tableName;
        private readonly AmazonDynamoDBClient _client;

        public DynamoKeyValueStore()
        {
            _tableName = Environment.GetEnvironmentVariable("DYNAMODB_TABLE_NAME");
            if (string.IsNullOrWhiteSpace(_tableName))
                throw new ApplicationException("No DynamoDB table name was provided!");

            _client = new AmazonDynamoDBClient();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue(key) }
                }
            };

            var response = await _client.GetItemAsync(request);
            if (!response.IsItemSet || !response.Item.ContainsKey("V"))
                return default(T);

            var json = response.Item["V"].S;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public Task SetAsync<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue(key) },
                    { "V", new AttributeValue(json) }
                }
            };

            return _client.PutItemAsync(request);
        }
    }
}
