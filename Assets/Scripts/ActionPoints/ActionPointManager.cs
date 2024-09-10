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

        [Header("Action Point Values")] [SerializeField, Tooltip("The minimum number of action points that a player can have at the start of their turn.")]
        int baseActionPoints;
        [SerializeField] int apPerPlanet;
        [SerializeField] int additionalApPerCapital;
        [SerializeField] int additionalApPerProdCenter;
        [SerializeField] int additionalApPerShipyard;


        [PropertySpace, ShowInInspector, ReadOnly]
        public int CurrentActionPoints { get; private set; }

        [HideInInspector] public List<ActionPointGenerator> actionPointGenerators = new List<ActionPointGenerator>();

        public int GetAPGeneratorValue(Planet planetInfo)
        {
            int totalApToGenerate = apPerPlanet;

            if (planetInfo.isShipyard) {
                totalApToGenerate += additionalApPerShipyard;
            }

            switch (planetInfo.planetSpecialty) {
            case PlanetSpecialty.Capital:
                totalApToGenerate += additionalApPerCapital;
                break;
            case PlanetSpecialty.ProductionCenter:
                totalApToGenerate += additionalApPerProdCenter;
                break;
            default:
                break;
            }

            return totalApToGenerate;
        }
        
        public void CalculateActionPoints()
        {
            int totalActionPoints = baseActionPoints;

            foreach (ActionPointGenerator thisGenerator in actionPointGenerators) {
                totalActionPoints += thisGenerator.APGeneratedPerTurn;
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
