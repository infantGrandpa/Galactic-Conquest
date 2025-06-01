using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class AttackAction : IGameAction
    {
        public int GetActionPointCost()
        {
            return 10;
        }

        public bool CanExecuteAction()
        {
            return true;
        }

        public bool ExecuteAction()
        {
            GUIManager.Instance.AddActionLogMessage("Executing attack action...");
            return true;
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
