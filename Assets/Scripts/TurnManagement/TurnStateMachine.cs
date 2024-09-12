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
