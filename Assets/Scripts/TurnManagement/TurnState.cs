using System.Collections;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    [System.Serializable]
    public abstract class TurnState
    {
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
