using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public interface ICacheHelper
    {       
        public string GetCache(string key);

        public void SetCache(KeyValuePair<string, string> keyValuePair);

        public void BuildCacheForBookHelper();


    }
}
