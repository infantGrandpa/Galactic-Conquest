using System.Collections.Generic;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class ActionPointManager : MonoBehaviour
    {
        public static ActionPointManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(ActionPointManager)) as ActionPointManager;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static ActionPointManager _instance;

        [Header("Action Point Values")] [SerializeField, Tooltip("The number of Action Points a player always gets at the start of their turn.")]
        int baseActionPoints;

        [PropertySpace, ShowInInspector, ReadOnly]
        public int CurrentActionPoints { get; private set; }

        [HideInInspector] public List<ActionPointAdjuster> actionPointAdjusters = new List<ActionPointAdjuster>();
        
        public void CalculateActionPoints()
        {
            int totalActionPoints = baseActionPoints;

            foreach (ActionPointAdjuster thisAdjuster in actionPointAdjusters) {
                totalActionPoints += thisAdjuster.TotalApPerTurn;
            }

            CurrentActionPoints = totalActionPoints;
            GUIManager.Instance.UpdateActionPoints(CurrentActionPoints);
        }

        public void IncreaseActionPoints(int increaseBy)
        {
            CurrentActionPoints += increaseBy;
            GUIManager.Instance.UpdateActionPoints(CurrentActionPoints);
        }

        public void DecreaseActionPoints(int decreaseBy)
        {
            CurrentActionPoints -= decreaseBy;
            GUIManager.Instance.UpdateActionPoints(CurrentActionPoints);
        }

        public bool IsTurnComplete()
        {
            if (CurrentActionPoints <= 0)
            {
                return true;
            }

            return false;
        }

        public bool CanPerformAction(int targetAPCost)
        {
            if (targetAPCost <= CurrentActionPoints)
            {
                return true;
            }

            return false;
        }
    }
}
