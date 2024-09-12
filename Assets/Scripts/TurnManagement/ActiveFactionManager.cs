using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Abraham.GalacticConquest.TurnManagement
{
    public class ActiveFactionManager : MonoBehaviour
    {
        public static ActiveFactionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(ActiveFactionManager)) as ActiveFactionManager;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static ActiveFactionManager _instance;

        [System.Serializable]
        public class FactionState
        {
            public Faction faction;
            public bool isActive;
            public int factionOrder;
            public FactionState(Faction faction, bool isActive, int factionOrder)
            {
                this.faction = faction;
                this.isActive = isActive;
                this.factionOrder = factionOrder;
            }
        }

        //We don't want anyone to be able to directly edit the FactionHandler list, we have tasks we need to do each time 1 gets added or removed.
        [ShowInInspector, ReadOnly] public List<FactionHandler> FactionHandlers { get; private set; } = new List<FactionHandler>();

        [ShowInInspector, ReadOnly] public List<FactionState> FactionStates { get; private set; } = new List<FactionState>();

        public void AddFactionHandlerToTurnList(FactionHandler factionHandlerToAdd)
        {
            if (FactionHandlers.Contains(factionHandlerToAdd)) {
                Debug.LogWarning("TurnStateMachine AddFactionHandlerToTurnList(): Faction Handler " + factionHandlerToAdd.gameObject.name + " is already in Faction Handler list.", this);
                return;
            }
            FactionHandlers.Add(factionHandlerToAdd);
            AddNewFaction(factionHandlerToAdd.myFaction);
        }

        public void RemoveFactionHandlerFromTurnList(FactionHandler factionHandlerToRemove)
        {
            if (!FactionHandlers.Contains(factionHandlerToRemove)) {
                Debug.LogWarning("TurnStateMachine RemoveFactionHandlerFromTurnList(): Faction Handler " + factionHandlerToRemove.gameObject.name + " is not in Faction Handler list.", this);
                return;
            }
            FactionHandlers.Remove(factionHandlerToRemove);
            CalculateActiveFactions();
        }

        void AddAllFactions()
        {
            foreach (FactionHandler thisFactionHandler in FactionHandlers) {
                Faction thisFaction = thisFactionHandler.myFaction;
                if (ActiveFactionsContains(thisFaction)) {
                    continue;
                }

                FactionState newFactionState = new FactionState(thisFaction, true, FactionStates.Count + 1);
                FactionStates.Add(newFactionState);
            }
        }

        FactionState AddNewFaction(Faction newFactionToCheck)
        {
            if (ActiveFactionsContains(newFactionToCheck, out FactionState newFactionState)) {
                return newFactionState;
            }

            newFactionState = new FactionState(newFactionToCheck, true, FactionStates.Count + 1);
            FactionStates.Add(newFactionState);
            return newFactionState;
        }

        [ContextMenu("Calculate Active Factions")]
        public void CalculateActiveFactions()
        {
            //Set all factions to inactive
            foreach (FactionState thisFactionState in FactionStates) {
                thisFactionState.isActive = false;
            }

            //Check FactionHandlers to determine which factions are active
            foreach (FactionHandler thisFactionHandler in FactionHandlers) {
                Faction thisFaction = thisFactionHandler.myFaction;
                FactionState thisFactionState = GetFactionStateByFaction(thisFaction);
                if (thisFactionState == null) {
                    thisFactionState = AddNewFaction(thisFaction);
                }

                thisFactionState.isActive = true;
            }
        }

        bool ActiveFactionsContains(Faction factionToTest)
        {
            return ActiveFactionsContains(factionToTest, out FactionState _);
        }

        bool ActiveFactionsContains(Faction factionToTest, out FactionState factionState)
        {
            factionState = GetFactionStateByFaction(factionToTest);
            if (factionState == null) {
                return false;
            }

            return true;
        }

        FactionState GetFactionStateByFaction(Faction faction)
        {
            foreach (FactionState thisFactionState in FactionStates) {
                if (thisFactionState.faction == faction) {
                    return thisFactionState;
                }
            }

            return null;
        }
    }
}
