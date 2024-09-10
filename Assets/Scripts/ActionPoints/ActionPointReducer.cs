using System;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class ActionPointReducer : MonoBehaviour
    {
        public int apCostPerTurn;

        void OnEnable()
        {
            ActionPointManager.Instance.actionPointReducers.Add(this);
        }

        void OnDisable()
        {
            if (ActionPointManager.Instance == null) {
                return;
            }
            ActionPointManager.Instance.actionPointReducers.Remove(this);
        }
    }
}
