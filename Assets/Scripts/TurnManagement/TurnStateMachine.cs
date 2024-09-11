using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.TurnManagement
{
    public class TurnStateMachine : MonoBehaviour
    {
        public static TurnStateMachine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(TurnStateMachine)) as TurnStateMachine;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static TurnStateMachine _instance;

        [ShowInInspector, ReadOnly] protected TurnState CurrentState;
        private Coroutine updateCoroutine;

        //We don't want anyone to be able to directly edit the FactionHandler list, we have tasks we need to do each time 1 gets added or removed.
        [ShowInInspector, ReadOnly] public List<FactionHandler> FactionHandlers { get; private set; } = new List<FactionHandler>();

        [ShowInInspector, ReadOnly] public List<Faction> activeFactions { get; private set; } = new List<Faction>();

        public void AddFactionHandlerToTurnList(FactionHandler factionHandlerToAdd)
        {
            if (FactionHandlers.Contains(factionHandlerToAdd)) {
                Debug.LogWarning("TurnStateMachine AddFactionHandlerToTurnList(): Faction Handler " + factionHandlerToAdd.gameObject.name + " is already in Faction Handler list.", this);
                return;
            }
            FactionHandlers.Add(factionHandlerToAdd);
            CalculateActiveFactions(factionHandlerToAdd.myFaction);
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

        public void CalculateActiveFactions()
        {
            foreach (FactionHandler thisFactionHandler in FactionHandlers) {
                Faction thisFaction = thisFactionHandler.myFaction;
                if (activeFactions.Contains(thisFaction)) {
                    continue;
                }

                activeFactions.Add(thisFaction);
            }
        }

        public void CalculateActiveFactions(Faction newFaction)
        {
            if (activeFactions.Contains(newFaction)) {
                return;
            }

            activeFactions.Add(newFaction);
        }

        private void Start()
        {
            SetState(new PlayerTurnState());
        }

        public void SetState(TurnState newState)
        {
            if (newState == null) {
                Debug.LogError("ERROR TurnStateMachine SetState(): newState is null.");
                return;
            }

            //Stop last state's update coroutine
            if (updateCoroutine != null) {
                StopCoroutine(updateCoroutine);
            }

            //Exit the current state
            if (CurrentState != null) {
                StartCoroutine(CurrentState.ExitState());
            }

            string currentStateName = CurrentState != null ? CurrentState.GetType().Name : "none";
            GUIManager.Instance.AddActionLogMessage("Changing state from " + currentStateName + " to " + newState.GetType().Name + "...");

            //Start new state
            CurrentState = newState;
            StartCoroutine(CurrentState.EnterState());

            //Update GUI elements
            GUIManager.Instance.ChangeTurn(CurrentState.GetType().Name);

            //Start the new state's updateCoroutine.
            //We can't do this in the update because it will start every frame, so here works for now.
            updateCoroutine = StartCoroutine(CurrentState.UpdateState());

        }
    }
}
