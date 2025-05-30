using System;
using System.Collections.Generic;
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
        private Stack<IGameAction> _actionHistory = new Stack<IGameAction>();

        public bool PerformAction(IGameAction action)
        {
            if (action.ExecuteAction())
            {
                _actionHistory.Push(action);
                return true;
            }

            return false;
        }

        public bool UndoLastAction()
        {
            throw new NotImplementedException();
        }
    }
}
