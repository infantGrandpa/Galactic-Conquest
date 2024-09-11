using System.Collections;
using Abraham.GalacticConquest.Factions;

namespace Abraham.GalacticConquest.TurnManagement
{
    [System.Serializable]
    public abstract class TurnState
    {
        public Faction faction;
        
        public virtual IEnumerator EnterState()
        {
            yield break;
        }
        public virtual IEnumerator UpdateState()
        {
            yield break;
        }
        public virtual IEnumerator ExitState()
        {
            yield break;
        }


    }
}
