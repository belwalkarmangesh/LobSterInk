﻿using Newtonsoft.Json;
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

            //Build Cache for Game 1 :
            if (string.IsNullOrWhiteSpace(GetCache("BookDilemaDecisionTreeSteps")) || string.IsNullOrEmpty(GetCache("BookDilemaDecisionTreeSteps")))
            {
                Dictionary<string, DecisionTreeQuery> doughnutAdventureSteps = new Dictionary<string, DecisionTreeQuery>();

                var queryAreYouSure
                  = new DecisionTreeQuery("Are you sure ?",
                                          "Buy it.",
                                          "You need to wait.",
                                          false,
                                          false);

                var queryIsItAGoodBook
                  = new DecisionTreeQuery("Is it a good book?",
                                          "What are you waiting for? Just buy it.",
                                          "Find another one.",
                                          false,
                                          false);
                var queryDoYouLikeIt
                  = new DecisionTreeQuery("Do you like it?",
                                          "Step-4",
                                          "Step-3",
                                          true,
                                          true);

                var queryDoYouWantABook
                  = new DecisionTreeQuery("Do you want a book?",
                                          "Step-2",
                                          "Maybe you want a pizza.",
                                          true,
                                          false);


                doughnutAdventureSteps.Add("Step-1", queryDoYouWantABook);
                doughnutAdventureSteps.Add("Step-2", queryDoYouLikeIt);
                doughnutAdventureSteps.Add("Step-3", queryIsItAGoodBook);
                doughnutAdventureSteps.Add("Step-4", queryAreYouSure);

                cachedSteps = JsonConvert.SerializeObject(doughnutAdventureSteps);

                KeyValuePair<string, string> keyValuePair = new("BookDilemaDecisionTreeSteps", cachedSteps);

                SetCache(keyValuePair);
            }
        }

        public void BuildCacheForDoughNutHelper()
        {
            //Build Cache for Game 2 :

        }

        #endregion Helper Methods for Building cache







    }
}