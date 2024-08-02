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
            List<Selectable> selectedObjects = new(SelectionManager.Instance.selectedObjects);
            if (selectedObjects.Count == 0)
            {
                return;
            }

            //Get Movement Point Cost
            List<Moveable> objectsToMove = new();
            int totalApCost = 0;
            foreach (Selectable thisSelectedObject in selectedObjects)
            {
                if (thisSelectedObject.TryGetComponent(out Moveable moveableObject))
                {
                    objectsToMove.Add(moveableObject);
                    totalApCost += moveableObject.movementApCost;
                }
            }

            //Cancel if not enough AP
            if (!ActionPointManager.Instance.CanPerformAction(totalApCost))
            {
                GUIManager.Instance.AddActionLogMessage("INSUFFICIENT AP (" + totalApCost + "): Movement Cancelled.");
                return;
            }


            //Get Move To Target
            RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor();

            if (nullableHitInfo == null)
            {
                return;
            }

            //Convert so we can get the transform of the hit object
            RaycastHit hitInfo = (RaycastHit)nullableHitInfo;

            //Get Selectable
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            if (!targetPlanet)
            {
                return;
            }

            //Send Move Command to selected objects
            foreach (Selectable thisObjectToMove in selectedObjects)
            {
                if (thisObjectToMove.TryGetComponent(out Moveable moveableObject))
                {
                    moveableObject.MoveToPlanet(targetPlanet);
                }
            }

            ActionPointManager.Instance.DecreaseActionPoints(totalApCost);
        }
    }
}