using Microsoft.AspNetCore.Mvc;
using OpenApi.Helpers;
using OpenApi.Models;

namespace OpenApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameHelper _gameHelper;

        public GamesController(IGameHelper gameHelper)
        {
            _gameHelper = gameHelper;

        }

        [HttpGet("[action]")]
        public string GetAllAdventures()
        {
            return "Here are all the Games : Game 1 - Book lover's Dilema ; Game 2 - Doughnut Decision Helper";
        }

        [HttpGet("[action]/{userInput}")]
        public GameResponse PlayGame(string userInput)
        {
            return _gameHelper.StepHelper(userInput);
        }

        [HttpGet("[action]")]
        public string GetCurrentGameStepsTaken()
        {
            return _gameHelper.GetUserGameStepTaken();
        }
    }
}
