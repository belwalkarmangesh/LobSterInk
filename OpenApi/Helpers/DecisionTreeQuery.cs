namespace OpenApi.Helpers
{
    public class DecisionTreeQuery
    {
        public string Sentence;
        public string PositiveStep;
        public string NegativeStep;

        public DecisionTreeQuery(string sentence,
                                 string positive,
                                 string negative)
        {
            Sentence = sentence;
            PositiveStep = positive;
            NegativeStep = negative;
        }

        public string EvaluateNextStep(string userKey)
        {
            while (true)
            {
                if (userKey == "Yes") 
                    return PositiveStep;
                else
                if (userKey == "No")
                    return NegativeStep;
                else
                    return string.Empty;
            }
        }
    }
   
}
