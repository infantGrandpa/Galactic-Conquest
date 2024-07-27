using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlayerTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            yield return new WaitForSeconds(2f);
            TurnStateMachine.Instance.SetState(new EnemyTurnState());
        }
    }
}
