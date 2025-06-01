using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.GUI;

namespace Abraham.GalacticConquest.Actions
{
    public class CompoundAction : IGameAction
    {

        private List<IGameAction> _actions;

        private int? _apCost = null;

        public CompoundAction(params IGameAction[] actions)
        {
            _actions = new List<IGameAction>(actions);
        }
        
        public int GetActionPointCost()
        {
            if (_apCost != null)
            {
                return _apCost.Value;
            }

            int apCost = 0;
            foreach (IGameAction action in _actions)
            {
                 apCost += action.GetActionPointCost();
            }

            return apCost;
        }

        public bool CanExecuteAction()
        {
            foreach (IGameAction action in _actions)
            {
                bool canExecute = action.CanExecuteAction();
                if (!canExecute)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ExecuteAction()
        {
            foreach (IGameAction action in _actions)
            {
                if (!action.ExecuteAction())
                {
                    // TODO: What happens if one of the actions fails? Right now we just exit.
                    //  Should it undo the other actions?
                    GUIManager.Instance.AddActionLogMessage($"{action.GetType()} action failed. Ending compound action.");
                    return false;
                }
            }

            return true;
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
