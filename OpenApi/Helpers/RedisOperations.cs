using StackExchange.Redis;
using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public class RedisOperations : IRedisOperations
    {
        private readonly IDatabase _database;

        public RedisOperations(IDatabase database)
        {
            _database = database;
        }

        public string GetRedisCache(string key)
        {
            return _database.StringGet(key);
        }

        public void SetRedisCache(KeyValuePair<string, string> keyValue)
        {
            _database.StringSet(keyValue.Key,keyValue.Value);
        }
    }
}
