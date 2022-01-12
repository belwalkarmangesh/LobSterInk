using Microsoft.AspNetCore.Mvc;
using OpenApi.Helpers;
using OpenApi.Models;
using System.Collections.Generic;

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
        public ActionResult<List<GetAllGames>> GetAllAdventures()
        { 
            return Ok(_gameHelper.GetAllGames());
        }

        [HttpGet("[action]/{userInput}")]
        public ActionResult<GameResponse> PlayGame(string userInput)
        {
            return Ok(_gameHelper.StepHelper(userInput));
        }

        [HttpGet("[action]")]
        public ActionResult<string> GetCurrentGameStepsTaken()
        {
            return Ok(_gameHelper.GetUserGameStepTaken());
        }
    }
}
