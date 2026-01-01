using System.Collections.Generic;
using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(ActionManager)) as ActionManager;

                return _instance;
            }
            set { _instance = value; }
        }

        private static ActionManager _instance;
        [ShowInInspector, ReadOnly, ListDrawerSettings(ShowFoldout = true)]
        private Stack<GameAction> _actionHistory = new();

        public bool PerformAction(GameAction action)
        {
            if (action.ExecuteAction())
            {
                _actionHistory.Push(action);
                return true;
            }

            return false;
        }

        public void ClearActionHistory()
        {
            _actionHistory.Clear();
        }

        public bool UndoLastAction()
        {
            if (_actionHistory.Count == 0)
            {
                GUIManager.Instance.AddActionLogMessage("No actions to undo.");
                return false;
            }

            GameAction lastActionToUndo = _actionHistory.Pop();
            return lastActionToUndo.UndoAction();
        }
    }
}