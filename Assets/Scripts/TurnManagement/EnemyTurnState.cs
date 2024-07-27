using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class EnemyTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            Debug.Log("Starting Enemy turn.");
            yield return new WaitForSeconds(3f);
            TurnStateMachine.Instance.SetState(new PlayerTurnState());
        }

        public override IEnumerator ExitState()
        {
            Debug.Log("Exiting Enemy turn.");
            yield break;
        }
    }
}
