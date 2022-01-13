﻿using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
    public class ScenarioFourTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public ScenarioFourTest()
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
        public void Test4()
        {
            //Do you want a book? ("Yes") => Do you like it ("No") => Is it a good book? ("Yes") => What are you waiting for? Just buy it..
            var gameResponse = PlayGameScenarioDontWaitTestHelper("Game 1");

            Assert.Equal(Constants.GameFinished, gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioDontWaitTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.DoYouLikeIt)
            {
                userInput = "No";
                return PlayGameScenarioDontWaitTestHelper(userInput);
            }

            if (gamesResponse.StepQuestion == Constants.IsItAGoodBook)
            {
                userInput = "Yes";
                return PlayGameScenarioDontWaitTestHelper(userInput);
            }
            if (gamesResponse.StepQuestion == Constants.JustBuyIt)
            {
                return new GameResponse(Constants.GameFinished, null);
            }
            userInput = "Yes";
            return PlayGameScenarioDontWaitTestHelper(userInput);
        }
    }
}
