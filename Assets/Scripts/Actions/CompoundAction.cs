using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;

namespace Abraham.GalacticConquest.Actions
{
    public class CompoundAction : GameAction
    {
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
            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                return false;
            }

            foreach (GameAction action in _actions)
            {
                bool canExecute = action.CanExecuteAction();
                if (!canExecute)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return false;
            }

            foreach (GameAction action in _actions)
            {
                if (action.ExecuteAction()) continue;
                
                // TODO: What happens if one of the actions fails? Right now we just exit.
                //  Should it undo the other actions?
                GUIManager.Instance.AddActionLogMessage($"{action.GetType()} action failed. Ending compound action.");
                return false;
            }

            return true;
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
                    $"Failed to undo {thisAction.GetType()} action at index {thisActionIndex} in compound action.");
                return false;
            }

            return true;
        }
    }
}