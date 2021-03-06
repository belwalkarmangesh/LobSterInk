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
            var steps = GetCacheForGame(Constants.KeyBookLoversDilema);

            if (userInput == "Game 1" || userInput == "Game 2")
            {
                nextStep = "Step-1";

                _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyGameSteps, null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyEndOfGame, null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyCurrentStep, nextStep));

                stepOutput = BuildOutputForGivenStep(steps, nextStep,currentStep,userInput);

                //Steps saving
                SaveSteps(steps, nextStep, currentStep, userInput);
            }
            else
            {
                if (string.IsNullOrEmpty(_cacheHelper.GetCache(Constants.KeyEndOfGame)))
                {
                    //Current Step from cache.
                    currentStep = _cacheHelper.GetCache(Constants.KeyCurrentStep);

                    //Get Next Step:
                    nextStep = steps[currentStep].EvaluateNextStep(userInput);

                    stepOutput = BuildOutputForGivenStep(steps, nextStep,currentStep,userInput);

                    _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyCurrentStep, nextStep));
                    
                }
                else
                {
                    return new GameResponse(Constants.PleaseStartNewGame, Constants.GameStartInputs);
                }
            }
            return stepOutput;
        }

        public string GetUserGameStepTaken()
        {

            return (!string.IsNullOrEmpty(_cacheHelper.GetCache(Constants.KeyGameSteps)) ? _cacheHelper.GetCache(Constants.KeyGameSteps) : Constants.NoGamePlayed);
        }
        public List<GetAllGames> GetAllGames()
        {
            var games = new List<GetAllGames>
            {
                new GetAllGames{ Name = "Game 1", Description = "Book lover's Dilema" },
                new GetAllGames { Name = "Game 2", Description = "Doughnut Helper" }
            };

            return games;
        }

        private GameResponse BuildOutputForGivenStep(Dictionary<string, DecisionTreeQuery> steps, string nextStep,string currentStep,string userInput)
        {
            //If Next step contains string step, find next step else return Next step

            if (nextStep.Contains("Step"))
            {
                var step = steps[nextStep];

                //Save Steps taken in Game
                SaveSteps(steps, nextStep, currentStep, userInput);

                return new GameResponse(step.Sentence, Constants.AllowedInputs);
            }
            else
            {
                _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyCurrentStep, null));

                _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyEndOfGame, "EndOfGame"));

                //Save Steps taken in Game
                SaveSteps(steps, nextStep, currentStep, userInput);

                return new GameResponse(nextStep, Constants.YouReachedEnfOfGame);
            }
        }

        private void SaveSteps(Dictionary<string, DecisionTreeQuery> steps, string nextStep, string currentStep, string userInput)
        {
            string savedSteps = string.Empty;

            if (!string.IsNullOrEmpty(_cacheHelper.GetCache(Constants.KeyGameSteps)))
            {
                saveSteps = JsonConvert.DeserializeObject<Dictionary<string, GameStepsTaken>>(_cacheHelper.GetCache(Constants.KeyGameSteps));
                if (nextStep.Contains("Step"))
                {
                    if (!(nextStep == "Step-1"))
                    {
                        saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = steps[currentStep].Sentence, UserSelection = "You selected " + userInput });
                    }
                }
                else
                {
                    saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = nextStep, UserSelection = Constants.YouReachedEnfOfGame });
                }
            }
            else
            {
                saveSteps.Add("Step " + (saveSteps.Count + 1), new GameStepsTaken() { StepQuestion = "'" + userInput + "' started..", UserSelection = "You selected " + userInput });
            }

            savedSteps = JsonConvert.SerializeObject(saveSteps);
            _cacheHelper.SetCache(new KeyValuePair<string, string>(Constants.KeyGameSteps, savedSteps));
        }

        
    }
}
