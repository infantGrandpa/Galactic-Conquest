using DG.Tweening;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] float lookTweenDuration = 0.5f;
        [SerializeField] float moveTweenDuration = 1f;

        public int movementApCost;

        public void MoveToPlanet(PlanetBehaviour targetPlanet)
        {
            Transform targetTransform = targetPlanet.fleetSlotTransform;

            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOLookAt(targetTransform.position, lookTweenDuration));
            moveSequence.Append(transform.DOMove(targetTransform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));
        }
    }
}