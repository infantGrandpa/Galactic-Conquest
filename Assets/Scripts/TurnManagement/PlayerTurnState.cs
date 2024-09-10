using System.Collections;
using Abraham.GalacticConquest.ActionPoints;

namespace Abraham.GalacticConquest.TurnManagement
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
