using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
    public class ScenarioOneTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public ScenarioOneTest()
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
            //Do you want a book? ("Yes") => Do you like it ("Yes") => Are you sure ? ("Yes") => Buy it.
            var gameResponse = PlayGameScenarioBuyItTestHelper("Game 1");

            Assert.Equal("Game Succesfully finished", gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioBuyItTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.BuyIt)
            {
                return new GameResponse("Game Succesfully finished", null);
            }
            userInput = "Yes";
            return PlayGameScenarioBuyItTestHelper(userInput);
        }
    }
}
