using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenApi;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
    public class ScenarioThreeTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public ScenarioThreeTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
            _database = redis.GetDatabase();
            _redisOperations = new RedisOperations(_database);
            cacheHelper = new CacheHelper(_redisOperations);
            _controller = new GamesController(_service);
        }


        [Fact]
        public void Test3()
        {
            //Do you want a book? ("Yes") => Do you like it ("Yes") => Are you sure ? ("No") => You need to wait.
            var gameResponse = PlayGameScenarioBuyItTestHelper("Game 1");

            Assert.Equal("Game Succesfully finished", gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioBuyItTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.AreYouSure)
            {
                userInput = "No";
                return PlayGameScenarioBuyItTestHelper(userInput);
            }

            if (gamesResponse.StepQuestion == Constants.PleaseWait)
            {
                return new GameResponse("Game Succesfully finished", null);
            }

            userInput = "Yes";
            return PlayGameScenarioBuyItTestHelper(userInput);
        }
    }
}
