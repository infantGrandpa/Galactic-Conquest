using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest.TurnManagement
{
    public class EnemyTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            yield return new WaitForSeconds(3f);
            TurnStateMachine.Instance.SetState(new PlayerTurnState());
        }
    }
}
