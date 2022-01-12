using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using OpenApi.Helpers;
using OpenApi.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using Xunit;

namespace TestProject
{
    [Collection("Sequential")]
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

        [Fact]
        public void Test1()
        {
            
            var result = _controller.GetAllAdventures();

            Assert.IsType<OkObjectResult>(result.Result);

            var gamesResult = result.Result as OkObjectResult;

            Assert.IsType<List<GetAllGames>>(gamesResult.Value);

            var gamesList = gamesResult.Value as List<GetAllGames>;

            Assert.Equal(2, gamesList.Count);
        }

        
          
    }
}
