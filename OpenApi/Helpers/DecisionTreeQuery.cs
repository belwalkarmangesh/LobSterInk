namespace OpenApi.Helpers
{
    public class DecisionTreeQuery
    {
        public string Sentence;
        public string PositiveStep;
        public string NegativeStep;
        public bool HasPositiveNextStep;
        public bool HasNegativeNextStep;

        public DecisionTreeQuery(string sentence,
                                 string positive,
                                 string negative,
                                 bool hasPositiveNextStep,
                                 bool hasNegatievNextStep)
        {
            Sentence = sentence;
            PositiveStep = positive;
            NegativeStep = negative;
            HasPositiveNextStep = hasPositiveNextStep;
            HasNegativeNextStep = hasNegatievNextStep;
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
