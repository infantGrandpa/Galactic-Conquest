using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlayerTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            Debug.Log("Starting Player turn.");
            yield return new WaitForSeconds(2f);
            TurnStateMachine.Instance.SetState(new EnemyTurnState());
        }

        public override IEnumerator ExitState()
        {
            Debug.Log("Exiting Player turn.");
            yield break;
        }
    }
}
