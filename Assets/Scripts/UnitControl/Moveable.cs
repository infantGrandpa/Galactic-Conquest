using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.UnitControl
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] float lookTweenDuration = 0.5f;
        [SerializeField] float moveTweenDuration = 1f;

        [FormerlySerializedAs("movementApCost"), SerializeField]
        int baseMovementApCost;

        [ReadOnly] public PlanetBehaviour currentPlanet = null;

        public virtual bool MoveToPlanet(PlanetBehaviour targetPlanet)
        {
            if (!CanMoveToTarget(targetPlanet))
            {
                return false;
            }

            Transform moveToTransform = targetPlanet.PlanetSlotHandler.AddMoveableToAvailableSlot(this);
            if (moveToTransform == null)
            {
                GUIManager.Instance.AddActionLogMessage("Planet occupied. Movement Cancelled.");
                return false;
            }

            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOLookAt(moveToTransform.position, lookTweenDuration));
            moveSequence.Append(transform.DOMove(moveToTransform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));

            ChangeCurrentPlanet(targetPlanet);
            return true;
        }

        public bool CanMoveToTarget(PlanetBehaviour targetPlanet)
        {
            if (targetPlanet == currentPlanet)
            {
                //We're already at this planet. Cancel.
                return false;
            }

            if (!targetPlanet.PlanetSlotHandler.AreAnySlotsAvailable())
            {
                //Planet is fully occupied. Cancel.
                GUIManager.Instance.AddActionLogMessage("Planet occupied. Movement Cancelled.");
                return false;
            }

            return true;
        }

        public int CalculateMovementCost(PlanetBehaviour targetPlanet)
        {
            return CalculateMovementCost(targetPlanet.transform.position);
        }

        public int CalculateMovementCost(Vector3 endPosition)
        {
            float distanceToPlanet = Vector3.Distance(transform.position, endPosition);

            int distanceApCost = Mathf.FloorToInt(distanceToPlanet / 10);   //TODO: Base distance off less magic numbers

            int totalApCost = baseMovementApCost + distanceApCost;

            return totalApCost;
        }

        public void ChangeCurrentPlanet(PlanetBehaviour newPlanet)
        {
            //Remove from old planet
            currentPlanet?.PlanetSlotHandler.RemoveMoveableFromSlot(this);

            //Move successful
            currentPlanet = newPlanet;
        }

        public void ChangeCurrentPlanet(GameObject newPlanetObject)
        {
            PlanetBehaviour newPlanetBehaviour = newPlanetObject.GetComponent<PlanetBehaviour>();
            if (newPlanetBehaviour == null) {
                Debug.LogError("ERROR Moveable ChangeCurrentPlanet(): " + newPlanetObject.gameObject + " is missing a PlanetBehaviour component.", this);
                return;
            }

            ChangeCurrentPlanet(newPlanetBehaviour);
        }
    }
}