using UnityEngine;
using System.Collections.Generic;

namespace Abraham.GalacticConquest
{
    public class MovementManager : MonoBehaviour
    {
        public static MovementManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(MovementManager)) as MovementManager;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static MovementManager instance;

        public List<PlanetBehaviour> planets = new();

        public void MoveToPlanet()
        {
            //Cancel if nothing is selected
            Selectable selectedObject = SelectionManager.Instance.selectedObject;
            if (selectedObject == null)
            {
                return;
            }

            //Get Movement Point Cost
            if (!selectedObject.TryGetComponent(out Moveable moveableObject))
            {
                //Object not movable. Cancel.
                return;
            }

            //Get Move To Target
            RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor();
            if (nullableHitInfo == null)
            {
                //Didn't click on anything. Cancel.
                return;
            }

            int totalApCost = moveableObject.movementApCost;
            if (!ActionPointManager.Instance.CanPerformAction(totalApCost))
            {
                //Not Enough AP. Cancel.
                GUIManager.Instance.AddActionLogMessage("INSUFFICIENT AP (" + totalApCost + "): Movement Cancelled.");
                return;
            }

            //Convert hit info so we can get the transform of the hit object
            RaycastHit hitInfo = (RaycastHit)nullableHitInfo;

            //Get Selectable
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            if (!targetPlanet)
            {
                return;
            }

            //Send Move Command to moveable object
            bool moveSuccessful = moveableObject.MoveToPlanet(targetPlanet);
            if (!moveSuccessful)
            {
                //Move cancelled by moveable object.
                //TODO: Check if we're already at the target planet earlier.
                return;
            }

            ActionPointManager.Instance.DecreaseActionPoints(totalApCost);
        }
    }
}