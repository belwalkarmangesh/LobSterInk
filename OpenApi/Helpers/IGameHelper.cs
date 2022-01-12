using OpenApi.Models;
using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public interface IGameHelper
    {
        public List<GetAllGames> GetAllGames();
        public GameResponse StepHelper(string userInput);
        public string GetUserGameStepTaken();
    }
}
