using Newtonsoft.Json;
using OpenApi.Models;
using System.Collections.Generic;

namespace OpenApi.Helpers
{
    public class GameHelper : IGameHelper
    {
        private readonly ICacheHelper _cacheHelper;
        public Dictionary<string, GameStepsTaken> saveSteps;
        public GameHelper(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
            _cacheHelper.BuildCacheForBookHelper();
            saveSteps = new Dictionary<string, GameStepsTaken>();
        }

        public Dictionary<string, DecisionTreeQuery> GetCacheForGame(string key)
        {
            var cacheString = _cacheHelper.GetCache(key);
            var cachedSteps = JsonConvert.DeserializeObject<Dictionary<string, DecisionTreeQuery>>(cacheString);
            return cachedSteps;
        }
        public GameResponse StepHelper(string userInput)
        {
            string currentStep = string.Empty;
            string nextStep = string.Empty;
            GameResponse stepOutput = null;
            var steps = GetCacheForGame("BookDilemaDecisionTreeSteps");

            if (userInput == "Game 1" || userInput == "Game 2")
            {
                nextStep = "Step-1";

                _cacheHelper.SetCache(new KeyValuePair<string, string>("GameSteps", null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>("EndOfGame", null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>("CurrentStep", nextStep));

                stepOutput = BuildOutputForGivenStep(steps, nextStep,currentStep,userInput);

                //Steps saving
                SaveSteps(steps, nextStep, currentStep, userInput);
            }
            else
            {
                if (string.IsNullOrEmpty(_cacheHelper.GetCache("EndOfGame")))
                {
                    //Current Step from cache.
                    currentStep = _cacheHelper.GetCache("CurrentStep");

                    //Get Next Step:
                    nextStep = steps[currentStep].EvaluateNextStep(userInput);

                    stepOutput = BuildOutputForGivenStep(steps, nextStep,currentStep,userInput);

                    _cacheHelper.SetCache(new KeyValuePair<string, string>("CurrentStep", nextStep));
                    
                }
                else
                {
                    return new GameResponse("The Game is ended. Please start a new Game to continue", "Allowed Inputs : 'Game 1' or 'Game 2'");
                }
            }
            return stepOutput;
        }      

        private GameResponse BuildOutputForGivenStep(Dictionary<string, DecisionTreeQuery> steps, string nextStep,string currentStep,string userInput)
        {
            //If Next step contains string step, find next step else return Next step

            if (nextStep.Contains("Step"))
            {
                var step = steps[nextStep];

                //Save Steps taken in Game
                SaveSteps(steps, nextStep, currentStep, userInput);

                return new GameResponse(step.Sentence, "Allowed Inputs : 'Yes' or 'No'");
            }
            else
            {
                _cacheHelper.SetCache(new KeyValuePair<string, string>("CurrentStep", null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>("EndOfGame", "EndOfGame"));

                //Save Steps taken in Game
                SaveSteps(steps, nextStep, currentStep, userInput);

                return new GameResponse(nextStep, "You reached End of the Game, Hope you enjoyed!");
            }
        }

        private void SaveSteps(Dictionary<string, DecisionTreeQuery> steps, string nextStep, string currentStep, string userInput)
        {
            string savedSteps = string.Empty;

            if (!string.IsNullOrEmpty(_cacheHelper.GetCache("GameSteps")))
            {
                saveSteps = JsonConvert.DeserializeObject<Dictionary<string, GameStepsTaken>>(_cacheHelper.GetCache("GameSteps"));
                if (nextStep.Contains("Step"))
                {
                    if (!(nextStep == "Step-1"))
                    {
                        saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = steps[currentStep].Sentence, UserSelection = "You selected " + userInput });
                    }
                }
                else
                {
                    saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = nextStep, UserSelection = "You reached End of the Game, Hope you enjoyed!" });
                }
            }
            else
            {
                saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = "'" + userInput + "' started..", UserSelection = "You selected " + userInput });
            }

            savedSteps = JsonConvert.SerializeObject(saveSteps);
            _cacheHelper.SetCache(new KeyValuePair<string, string>("GameSteps", savedSteps));
        }

        public string GetUserGameStepTaken()
        {
           return _cacheHelper.GetCache("GameSteps");
        }
    }
}
