using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class TurnStateMachine : MonoBehaviour
    {
        public static TurnStateMachine Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(TurnStateMachine)) as TurnStateMachine;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static TurnStateMachine instance;

        [ShowInInspector, ReadOnly] protected TurnState currentState;
        private Coroutine updateCoroutine;

        private void Start()
        {
            SetState(new PlayerTurnState());
        }

        public void SetState(TurnState newState)
        {
            if (newState == null)
            {
                Debug.LogError("ERROR TurnStateMachine SetState(): newState is null.");
                return;
            }

            //Stop last state's update coroutine
            if (updateCoroutine != null)
            {
                StopCoroutine(updateCoroutine);
            }

            //Exit the current state
            if (currentState != null)
            {
                StartCoroutine(currentState.ExitState());
            }

            string currentStateName = currentState != null ? currentState.GetType().Name : "none";
            GUIManager.Instance.AddActionLogMessage("Changing state from " + currentStateName + " to " + newState.GetType().Name + "...");

            //Start new state
            currentState = newState;
            StartCoroutine(currentState.EnterState());

            //Update GUI elements
            GUIManager.Instance.ChangeTurn(currentState.GetType().Name);

            //Start the new state's updateCorutin.
            //We can't do this in the update because it will start every frame, so here works for now.
            updateCoroutine = StartCoroutine(currentState.UpdateState());

        }
    }
}
