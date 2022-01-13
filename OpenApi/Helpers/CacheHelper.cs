using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IRedisOperations _redisOperations;
        public CacheHelper(IRedisOperations redisOperations)
        {
            _redisOperations = redisOperations;
        }

        #region Methods for Primary cache Operations
        public string GetCache(string key)
        {

            return _redisOperations.GetRedisCache(key);
        }

        public void SetCache(KeyValuePair<string, string> keyValuePair)
        {
            _redisOperations.SetRedisCache(keyValuePair);
        }

        #endregion End of Primary Operation

        #region Helper Methods for Building cache
        public void BuildCacheForBookHelper()
        {
            string cachedSteps = string.Empty;

            //Build Cache for Game 2 :
            if (string.IsNullOrWhiteSpace(GetCache(Constants.KeyBookLoversDilema)) || string.IsNullOrEmpty(GetCache(Constants.KeyBookLoversDilema)))
            {
                var doughnutAdventureSteps = new Dictionary<string, DecisionTreeQuery>();

                var queryAreYouSure
                  = new DecisionTreeQuery(Constants.AreYouSure,
                                          Constants.BuyIt,
                                          Constants.PleaseWait);

                var queryIsItAGoodBook
                  = new DecisionTreeQuery(Constants.IsItAGoodBook,
                                          Constants.JustBuyIt,
                                          Constants.FindAnotherOne);
                var queryDoYouLikeIt
                  = new DecisionTreeQuery(Constants.DoYouLikeIt,
                                          "Step-4",
                                          "Step-3");

                var queryDoYouWantABook
                  = new DecisionTreeQuery(Constants.DoYouWantBook,
                                          "Step-2",
                                          Constants.MayBeYouWantPizza);


                doughnutAdventureSteps.Add("Step-1", queryDoYouWantABook);
                doughnutAdventureSteps.Add("Step-2", queryDoYouLikeIt);
                doughnutAdventureSteps.Add("Step-3", queryIsItAGoodBook);
                doughnutAdventureSteps.Add("Step-4", queryAreYouSure);

                cachedSteps = JsonConvert.SerializeObject(doughnutAdventureSteps);

                KeyValuePair<string, string> keyValuePair = new(Constants.KeyBookLoversDilema, cachedSteps);

                SetCache(keyValuePair);
            }
        }

        public void BuildCacheForDoughNutHelper()
        {
            //Build Cache for Game 1 :

        }

        #endregion Helper Methods for Building cache







    }
}
