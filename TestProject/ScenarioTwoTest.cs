using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
    public class ScenarioTwoTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public ScenarioTwoTest()
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
        public void Test2()
        {
            //Do you want a book? ("No") => Maybe you want a pizza..
            var gameResponse = PlayGameScenarioWantAPizzaTestHelper("Game 1");

            Assert.Equal(Constants.GameFinished, gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioWantAPizzaTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.MayBeYouWantPizza)
            {
                return new GameResponse(Constants.GameFinished, null);
            }
            userInput = "No";
            return PlayGameScenarioWantAPizzaTestHelper(userInput);
        }
    }
}
