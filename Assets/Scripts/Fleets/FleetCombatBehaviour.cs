using DG.Tweening;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetCombatBehaviour : MonoBehaviour
    {
        private Moveable myMoveable;

        private void Awake()
        {
            myMoveable = GetComponent<Moveable>();
        }
    }
}