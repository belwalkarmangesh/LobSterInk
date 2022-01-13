using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using Xunit;

namespace TestProject
{
    [TestCaseOrderer("TestProject.PriorityOrderer", "TestProject")]
    public class GamesControllerTest
    {
        GamesController _controller;
        IGameHelper _service;
        ICacheHelper cacheHelper;
        IRedisOperations _redisOperations;
        IDatabase _database;
        public GamesControllerTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
            _database = redis.GetDatabase();
            _redisOperations = new RedisOperations(_database);
            cacheHelper = new CacheHelper(_redisOperations);
            _service = new GameHelper(cacheHelper);
            _controller = new GamesController(_service);

        }

        [Fact, TestPriority(1)]
        public void Test1()
        {

            var result = _controller.GetAllAdventures();

            Assert.IsType<OkObjectResult>(result.Result);

            var gamesResult = result.Result as OkObjectResult;

            Assert.IsType<List<GetAllGames>>(gamesResult.Value);

            var gamesList = gamesResult.Value as List<GetAllGames>;

            Assert.Equal(2, gamesList.Count);
        }



        [Fact, TestPriority(2)]
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
        [Fact, TestPriority(3)]
        public void Test3()
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

        [Fact, TestPriority(4)]
        public void Test4()
        {
            //Do you want a book? ("Yes") => Do you like it ("Yes") => Are you sure ? ("No") => You need to wait.
            var gameResponse = PlayGameScenarioYouNeedToWaitTestHelper("Game 1");

            Assert.Equal("Game Succesfully finished", gameResponse.StepQuestion);
        }

        private GameResponse PlayGameScenarioYouNeedToWaitTestHelper(string userInput)
        {
            var result = _controller.PlayGame(userInput);

            var gamesResult = result.Result as OkObjectResult;

            var gamesResponse = gamesResult.Value as GameResponse;

            if (gamesResponse.StepQuestion == Constants.AreYouSure)
            {
                userInput = "No";
                return PlayGameScenarioYouNeedToWaitTestHelper(userInput);
            }

            if (gamesResponse.StepQuestion == Constants.PleaseWait)
            {
                return new GameResponse("Game Succesfully finished", null);
            }

            userInput = "Yes";
            return PlayGameScenarioYouNeedToWaitTestHelper(userInput);
        }

        [Fact, TestPriority(5)]
        public void Test5()
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

        [Fact, TestPriority(6)]
        public void Test6()
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
