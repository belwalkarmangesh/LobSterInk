using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
    public class ScenarioFiveTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public ScenarioFiveTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
            _database = redis.GetDatabase();
            _redisOperations = new RedisOperations(_database);
            cacheHelper = new CacheHelper(_redisOperations);
            _service = new GameHelper(cacheHelper);
            _controller = new GamesController(_service);
        }


        [Fact]
        public void Test5()
        {
            //Do you want a book? ("Yes") => Do you like it ("No") => Is it a good book? ("No") => Find another one..
            var gameResponse = PlayGameScenarioFindAnotherTestHelper("Game 1");

            Assert.Equal(Constants.GameFinished, gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioFindAnotherTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.DoYouLikeIt)
            {
                userInput = "No";
                return PlayGameScenarioFindAnotherTestHelper(userInput);
            }

            if (gamesResponse.StepQuestion == Constants.IsItAGoodBook)
            {
                userInput = "No";
                return PlayGameScenarioFindAnotherTestHelper(userInput);
            }
            if (gamesResponse.StepQuestion == Constants.FindAnotherOne)
            {
                return new GameResponse(Constants.GameFinished, null);
            }
            userInput = "Yes";
            return PlayGameScenarioFindAnotherTestHelper(userInput);
        }
    }
}
