using JetBrains.Annotations;

namespace Abraham.GalacticConquest.Actions
{
    public class ActionResult
    {
        public string ActionType { get; private set; }
        public string Message { get; private set; }
        public int APCost { get; private set; }
        public bool WasSuccessful { get; private set; }

        private ActionResult(string actionType, bool wasSuccessful, int apCost, string message)
        {
            ActionType = actionType;
            WasSuccessful = wasSuccessful;
            Message = message;
            APCost = apCost;
        }

        public static ActionResult Success(string actionType, int apCost, [CanBeNull] string message = "")
        {
            return new ActionResult(actionType, true, apCost, message);
        }

        public static ActionResult Failure(string actionType, [CanBeNull] string message = "")
        {
            return new ActionResult(actionType, false, 0, message);
        }
    }
}
