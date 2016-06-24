namespace OneScanWebApp.PayloadObjects
{
    class ProcessOutcomePayload
    {
        public bool Success = false;
        public string MessageType = OutcomeTypes.LoginProblem.ToString();
    }

    enum OutcomeTypes
    {
        ProcessComplete,
        LoginProblem,
        RegisterUser
    }

    
}