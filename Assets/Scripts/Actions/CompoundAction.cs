using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;

namespace Abraham.GalacticConquest.Actions
{
    public class CompoundAction : IGameAction
    {
        private readonly List<IGameAction> _actions;

        private int? _apCost;

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
            _apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(_apCost.Value))
            {
                return false;
            }
            
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
            if (!CanExecuteAction())
            {
                return false;
            }
            
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

        public void AddAction(IGameAction action)
        {
            _actions.Add(action);
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
