using JetBrains.Annotations;
using Sirenix.OdinInspector;

namespace Abraham.GalacticConquest.Actions
{
    public class ActionResult
    {
        [ShowInInspector, ReadOnly] public string ActionType { get; private set; }
        [ShowInInspector, ReadOnly] public string Message { get; private set; }
        [ShowInInspector, ReadOnly] public int APCost { get; private set; }
        [ShowInInspector, ReadOnly] public bool WasSuccessful { get; private set; }

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
