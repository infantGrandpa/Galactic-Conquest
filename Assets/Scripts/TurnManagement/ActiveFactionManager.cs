using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Sirenix.OdinInspector;
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

        [ShowInInspector, ReadOnly] public List<FactionState> activeFactions { get; private set; } = new List<FactionState>();

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
            AddNewFaction();
        }

        public void AddNewFaction()
        {
            foreach (FactionHandler thisFactionHandler in FactionHandlers) {
                Faction thisFaction = thisFactionHandler.myFaction;
                if (ActiveFactionsContains(thisFaction)) {
                    continue;
                }

                FactionState newFactionState = new FactionState(thisFaction, true, activeFactions.Count + 1);
                activeFactions.Add(newFactionState);
            }
        }

        public void AddNewFaction(Faction newFactionToCheck)
        {
            if (ActiveFactionsContains(newFactionToCheck)) {
                return;
            }

            FactionState newFactionState = new FactionState(newFactionToCheck, true, activeFactions.Count + 1);
            activeFactions.Add(newFactionState);
        }

        private bool ActiveFactionsContains(Faction factionToTest)
        {
            foreach (FactionState thisFactionState in activeFactions) {
                if (thisFactionState.faction == factionToTest) {
                    return true;
                }
            }

            return false;
        }
    }
}
