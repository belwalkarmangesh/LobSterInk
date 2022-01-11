namespace OpenApi.Models
{
    public class GameResponse: BaseResponse
    {
        public GameResponse(string stepQuestion,
                                 string allowedInputMessage)
        {
            StepQuestion = stepQuestion;
            AllowedInputMessage = allowedInputMessage;
        }       

        public string AllowedInputMessage { get; set; }
    }
}
