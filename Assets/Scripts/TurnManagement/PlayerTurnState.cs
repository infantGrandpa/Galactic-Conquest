using System.Collections;
using Abraham.GalacticConquest.ActionPoints;

namespace Abraham.GalacticConquest.TurnManagement
{
    public class PlayerTurnState : TurnState
    {
        public override IEnumerator EnterState()
        {
            // This script isn't currently in use.
            // I've change CalculateActionPoints to require a Faction to calculate, which breaks this use.
            // Instead of fixing it, I'm commenting it out for now.
            
            //ActionPointManager.Instance.CalculateActionPoints();  
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