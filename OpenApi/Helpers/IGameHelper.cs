using OpenApi.Models;
namespace OpenApi.Helpers
{
    public interface IGameHelper
    {
        public GameResponse StepHelper(string userInput);
        public string GetUserGameStepTaken();
    }
}
