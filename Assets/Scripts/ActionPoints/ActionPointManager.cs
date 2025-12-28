using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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
            set { _instance = value; }
        }

        private static ActionPointManager _instance;

        [Header("Action Point Values")]
        [SerializeField, Tooltip("The number of Action Points a player always gets at the start of their turn.")]
        private int baseActionPoints;

        [Tooltip("The AP cost to build a new fleet.")]
        public int buildShipApCost;

        [Tooltip("The AP cost to attack a fleet or a planet.")]
        public int attackApCost;

        [Tooltip("The AP cost to fortify a planet.")]
        public int fortifyPlanetCost;

        [FormerlySerializedAs("CurrentActionPoints")] [PropertySpace, ShowInInspector, ReadOnly]
        public int currentActionPoints;

        [FormerlySerializedAs("actionPointModifiers")] [FormerlySerializedAs("actionPointAdjusters")] [HideInInspector]
        public List<ActionPointAggregator> actionPointAggregators = new();

        // readonly refers to the Dictionary itself, not the contents of the dictionary I guess
        private readonly Dictionary<Faction, List<ActionPointModifier>> _factionApModifiers = new();
        private readonly Dictionary<Faction, int> _factionRolloverPoints = new(); //TODO: Implement a not shit version of rollover points

        [SerializeField, Space] private float percOfRolloverPoints = 0.5f;

        public void CalculateActionPoints(Faction currentFaction)
        {
            BuildFactionApModifierList(); // TODO: Do we need to build this EVERY TIME we calculate AP?
            int totalActionPoints = baseActionPoints;

            if (!_factionApModifiers.TryGetValue(currentFaction, out List<ActionPointModifier> factionMods))
            {
                Debug.LogWarning($"No AP Modifiers found for faction {currentFaction.name}. Defaulting to base AP.");
                goto FactionNotFound;
            }

            foreach (ActionPointModifier thisMod in factionMods)
            {
                totalActionPoints += thisMod.apModificationValue;
            }

            if (_factionRolloverPoints.TryGetValue(currentFaction, out int rolloverPoints))
            {
                GUIManager.Instance.AddActionLogMessage($"Adding {rolloverPoints} points to {currentFaction.factionName} from last turn.");
                totalActionPoints += rolloverPoints;

                // Optionally clear it right away if you want them only used once:
                _factionRolloverPoints[currentFaction] = 0;
            }

            FactionNotFound:
            currentActionPoints = totalActionPoints;
            GUIManager.Instance.UpdateActionPoints(currentActionPoints);
        }

        [Button("Build AP Modifier List for each Faction")]
        private void BuildFactionApModifierList()
        {
            _factionApModifiers.Clear();

            foreach (ActionPointAggregator aggregator in actionPointAggregators)
            {
                //Get Aggregator Faction
                Faction aggregatorFaction = aggregator.ApFactionHandler?.myFaction;
                if (!aggregatorFaction)
                {
                    Debug.LogWarning(
                        $"ActionPointManager BuildFactionApModifierList(): ActionPointAggregator {aggregator.gameObject.name} does not have a Faction.",
                        this);
                    continue;
                }

                //Add Faction to dictionary if necessary
                if (!_factionApModifiers.ContainsKey(aggregatorFaction))
                {
                    _factionApModifiers[aggregatorFaction] = new List<ActionPointModifier>();
                }

                //Add each modifier within this aggregator to the faction
                List<ActionPointModifier> modifiersAtThisAggregator = aggregator.APModifiers;
                foreach (ActionPointModifier thisModifier in modifiersAtThisAggregator)
                {
                    _factionApModifiers[aggregatorFaction].Add(thisModifier);
                }
            }
        }

        public void SaveRolloverPoints(Faction faction)
        {
            int rollover = Mathf.FloorToInt(currentActionPoints * percOfRolloverPoints);
            _factionRolloverPoints[faction] = rollover;
            GUIManager.Instance.AddActionLogMessage($"Saved {rollover} of {currentActionPoints} points for the {faction.factionName}.");
        }


        public void IncreaseActionPoints(int increaseBy)
        {
            currentActionPoints += increaseBy;
            GUIManager.Instance.UpdateActionPoints(currentActionPoints);
        }

        public void DecreaseActionPoints(int decreaseBy)
        {
            //TODO: Should we test if we can actually decrease by this amount first?
            currentActionPoints -= decreaseBy;
            GUIManager.Instance.UpdateActionPoints(currentActionPoints);
        }

        public bool IsTurnComplete()
        {
            if (currentActionPoints <= 0)
            {
                return true;
            }

            return false;
        }

        public bool CanPerformAction(int targetAPCost)
        {
            return targetAPCost <= currentActionPoints;
        }
    }
}