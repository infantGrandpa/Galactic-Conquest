using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest
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

        [SerializeField] int baseActionPoints;
        [ShowInInspector, ReadOnly] public int CurrentActionPoints { get; private set; }

        public void CalculateActionPoints()
        {
            CurrentActionPoints = baseActionPoints;
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
