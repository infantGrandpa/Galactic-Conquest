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

            if (currentState != null)
            {
                StartCoroutine(currentState.ExitState());
            }

            currentState = newState;
            StartCoroutine(currentState.EnterState());

            GUIManager.Instance.ChangeTurn(currentState.GetType().Name);
        }
    }
}
