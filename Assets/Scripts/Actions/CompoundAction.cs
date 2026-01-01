using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;

namespace Abraham.GalacticConquest.Actions
{
    public class CompoundAction : GameAction
    {
        [ShowInInspector, ReadOnly]
        private readonly List<GameAction> _actions;

        public CompoundAction(params GameAction[] actions)
        {
            _actions = new List<GameAction>(actions);
        }

        protected override int CalculateActionPointCost()
        {
            int apCost = 0;
            foreach (GameAction action in _actions)
            {
                apCost += action.GetActionPointCost();
            }

            return apCost;
        }

        public override bool CanExecuteAction()
        {
            foreach (GameAction action in _actions)
            {
                bool canExecute = action.CanExecuteAction();
                if (!canExecute)
                {
                    Result = ActionResult.Failure(GetActionTypeName(), $"{action.GetActionTypeName()} cannot be executed.");
                    return Result.WasSuccessful;
                }
            }
            
            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                string msg = $"These actions require {apCost} AP.";
                GUIManager.Instance.AddActionLogMessage(msg);
                
                Result = ActionResult.Failure(GetActionTypeName(), msg);
                return Result.WasSuccessful;
            }

            return true;
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return Result.WasSuccessful;
            }

            string completedActionsMessage = "";
            int completedActionCount = 0;
            foreach (GameAction action in _actions)
            {
                if (action.ExecuteAction())
                {
                    if (completedActionCount > 0) completedActionsMessage += " => ";

                    completedActionsMessage += action.GetActionTypeName();
                    completedActionCount++;
                    continue;
                }
                
                // TODO: What happens if one of the actions fails? Right now we just exit.
                //  Should it undo the other actions?
                string msg = $"{action.GetActionTypeName()} action failed. Ending compound action.";
                
                GUIManager.Instance.AddActionLogMessage(msg);
                Result = ActionResult.Failure(GetActionTypeName(), msg);
                return Result.WasSuccessful;
            }

            Result = ActionResult.Success(GetActionTypeName(), GetActionPointCost(), $"Completed: {completedActionsMessage}");
            return Result.WasSuccessful;
        }

        public void AddAction(GameAction action)
        {
            _actions.Add(action);
        }

        public override bool UndoAction()
        {
            // Iterate through actions in reverse order to properly undo the compound action
            for (int thisActionIndex = _actions.Count - 1; thisActionIndex >= 0; thisActionIndex--)
            {
                GameAction thisAction = _actions[thisActionIndex];
                bool success = thisAction.UndoAction();

                if (success) continue;
                
                GUIManager.Instance.AddActionLogMessage(
                    $"Failed to undo {thisAction.GetActionTypeName()} action at index {thisActionIndex} in compound action.");
                return false;
            }

            return true;
        }
    }
}