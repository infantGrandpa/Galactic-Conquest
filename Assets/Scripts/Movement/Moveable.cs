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

            PlanetSlot availablePlanetSlot = targetPlanet.GetAvailablePlanetSlot();
            if (availablePlanetSlot == null)
            {
                GUIManager.Instance.AddActionLogMessage("Planet already occupied. Movement Cancelled.");
                return false;
            }

            Transform moveToTransform = availablePlanetSlot.SetOccupyingObject(this);
            if (moveToTransform == null)
            {
                GUIManager.Instance.AddActionLogMessage("Move to transform is null. Movement Cancelled.");
                return false;
            }

            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOLookAt(moveToTransform.position, lookTweenDuration));
            moveSequence.Append(transform.DOMove(moveToTransform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));

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