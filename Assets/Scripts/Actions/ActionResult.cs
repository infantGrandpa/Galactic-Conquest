using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class ActionResult
    {
        public string ActionType { get; private set; }

        [ShowInInspector, ReadOnly, Title("$ActionType")] public int APCost { get; private set; }
        public bool WasSuccessful { get; private set; }
        
        [ShowInInspector, ReadOnly, HideLabel, DisplayAsString(false, TextAlignment.Center, FontSize = 16)]
        public string Message { get; private set; }

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