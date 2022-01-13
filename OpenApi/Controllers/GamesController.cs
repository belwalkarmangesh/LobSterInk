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

        /// <summary>
        /// Shows currently available Adventures:
        /// </summary>
        /// <remarks>
        ///     FOr demo purpose we are listing 2 games:
        ///     1: Game 1 - Book Lover's Dilema
        ///     2: Doughnut Decision Helper
        ///     
        /// </remarks>
        [HttpGet("[action]")]
        public ActionResult<List<GetAllGames>> GetAllAdventures()
        { 
            return Ok(_gameHelper.GetAllGames());
        }

        [HttpGet("[action]/{gameName}")]
        public ActionResult<GameResponse> PlayGame(string gameName)
        {
            return Ok(_gameHelper.StepHelper(gameName));
        }

        [HttpGet("[action]")]
        public ActionResult<string> GetCurrentGameStepsTaken()
        {
            return Ok(_gameHelper.GetUserGameStepTaken());
        }
    }
}
