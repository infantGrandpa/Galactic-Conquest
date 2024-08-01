using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlayerTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            ActionPointManager.Instance.CalculateActionPoints();
            yield break;
        }

        public override IEnumerator UpdateState()
        {
            do
            {
                if (ActionPointManager.Instance.IsTurnComplete())
                {
                    TurnStateMachine.Instance.SetState(new EnemyTurnState());
                }

                yield return null;
            } while (true);
        }
    }
}
