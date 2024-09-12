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

        private void Start()
        {
            // TODO: Add a delay before starting the first turn state.
            // This is causing an issue on the first turn. The ActionPointAdjusters haven't received all their adjustments yet.
            // This means that first turn AP is only equal to the base AP per turn.
            // Adding a delay here would allow us to do anything we need to in order to set up before the turn starts.
            // It might also be a good idea to have this "delay" state to run before every turn to allow everything to calculate.
            
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
