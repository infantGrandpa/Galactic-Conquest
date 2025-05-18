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

        public int buildShipApCost;

        [PropertySpace, ShowInInspector, ReadOnly]
        public int CurrentActionPoints { get; private set; }

        [FormerlySerializedAs("actionPointModifiers")] [FormerlySerializedAs("actionPointAdjusters")] [HideInInspector]
        public List<ActionPointAggregator> actionPointAggregators = new();

        // readonly refers to the Dictionary itself, not the contents of the dictionary I guess
        private readonly Dictionary<Faction, List<ActionPointModifier>> _factionApModifiers = new();


        public void CalculateActionPoints()
        {
            int totalActionPoints = baseActionPoints;

            foreach (ActionPointAggregator aggregator in actionPointAggregators)
            {
                totalActionPoints += aggregator.TotalApPerTurn;
            }

            CurrentActionPoints = totalActionPoints;
            GUIManager.Instance.UpdateActionPoints(CurrentActionPoints);
        }

        [Button("Build AP Modifier List for each Faction")]
        private void BuildFactionApModifierList()
        {
            _factionApModifiers.Clear();

            foreach (ActionPointAggregator aggregator in actionPointAggregators)
            {
                Faction aggregatorFaction = aggregator.ApFactionHandler?.myFaction;
                if (!aggregatorFaction)
                {
                    Debug.LogWarning(
                        $"ActionPointManager BuildFactionApModifierList(): ActionPointAggregator {aggregator.gameObject.name} does not have a Faction.",
                        this);
                    continue;
                }

                if (!_factionApModifiers.ContainsKey(aggregatorFaction))
                {
                    _factionApModifiers[aggregatorFaction] = new List<ActionPointModifier>();
                }

                List<ActionPointModifier> modifiersAtThisAggregator = aggregator.APModifiers;
                foreach (ActionPointModifier thisModifier in modifiersAtThisAggregator)
                {
                    _factionApModifiers[aggregatorFaction].Add(thisModifier);
                }
            }

            foreach (var kvp in _factionApModifiers)
            {
                Faction faction = kvp.Key;
                List<ActionPointModifier> modifiers = kvp.Value;

                Debug.Log($"Faction: {faction.name} has {modifiers.Count} AP Modifiers:");

                int totalAp = baseActionPoints;
                foreach (ActionPointModifier mod in modifiers)
                {
                    Debug.Log($"  • Reason: {mod.apModificationReason}, Value: {mod.apModificationValue}");
                    totalAp += mod.apModificationValue;
                }

                Debug.Log($"Total {faction.factionName} AP: {totalAp}", this);
            }
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