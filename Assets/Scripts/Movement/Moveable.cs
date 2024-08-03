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
            if (!CanMoveToTarget(targetPlanet))
            {
                return false;
            }

            Transform moveToTransform = targetPlanet.AddMoveableToAvailableSlot(this);
            if (moveToTransform == null)
            {
                GUIManager.Instance.AddActionLogMessage("Planet occupied. Movement Cancelled.");
                return false;
            }

            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOLookAt(moveToTransform.position, lookTweenDuration));
            moveSequence.Append(transform.DOMove(moveToTransform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));

            //Remove from old planet
            currentPlanet?.RemoveMoveableFromSlot(this);

            //Move successful
            currentPlanet = targetPlanet;
            return true;
        }

        public bool CanMoveToTarget(PlanetBehaviour targetPlanet)
        {
            if (targetPlanet == currentPlanet)
            {
                //We're already at this planet. Cancel.
                return false;
            }

            return true;
        }
    }
}