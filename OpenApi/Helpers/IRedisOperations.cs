using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public interface IRedisOperations
    {
        //public void SetRedisCache(string key, string value);

        public void SetRedisCache(KeyValuePair<string,string> keyValue);

        public string GetRedisCache(string key);
    }
}
