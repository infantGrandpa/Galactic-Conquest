namespace Abraham.GalacticConquest.Actions
{
    public class ActionResult
    {
        public string ActionType { get; private set; }
        public string Message { get; private set; }
        public int APCost { get; private set; }
        public bool WasSuccessful { get; private set; }

        public ActionResult(string actionType, bool wasSuccessful, string message, int apCost)
        {
            ActionType = actionType;
            WasSuccessful = wasSuccessful;
            Message = message;
            APCost = apCost;
        }

        public static ActionResult Success(string actionType, string message, int apCost)
        {
            return new ActionResult(actionType, true, message, apCost);
        }

        public static ActionResult Failure(string actionType, string message)
        {
            return new ActionResult(actionType, false, message, 0);
        }
    }
}
