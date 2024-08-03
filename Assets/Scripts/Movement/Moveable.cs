using DG.Tweening;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] float lookTweenDuration = 0.5f;
        [SerializeField] float moveTweenDuration = 1f;

        public int movementApCost;

        private PlanetBehaviour currentPlanet = null;

        public bool MoveToPlanet(PlanetBehaviour targetPlanet)
        {
            if (targetPlanet == currentPlanet)
            {
                //We're already at this planet. Cancel.
                return false;
            }

            Transform targetTransform = targetPlanet.fleetSlotTransform;

            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOLookAt(targetTransform.position, lookTweenDuration));
            moveSequence.Append(transform.DOMove(targetTransform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));

            //Move successful
            currentPlanet = targetPlanet;
            return true;
        }
    }
}