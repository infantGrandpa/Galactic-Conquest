using Abraham.GalacticConquest.Refs;
using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
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
            LayerMask planetLayerMask = LayerMaskRefs.GetLayerMask(LayerMaskRefs.PlanetLayer);
            RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor(planetLayerMask);
            if (nullableHitInfo == null)
            {
                //Didn't click on anything. Cancel.
                return;
            }


            //Get target planet
            RaycastHit hitInfo = (RaycastHit)nullableHitInfo;   //Convert hit info so we can get the transform of the hit object
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            if (!targetPlanet)
            {
                //Didn't click on a planet. Cancel.
                return;
            }

            bool canMove = moveableObject.CanMoveToTarget(targetPlanet);
            if (!canMove)
            {
                //Moveable object already at planet. Cancel.
                return;
            }

            //Check AP costs. 
            // This is last so we don't send a message about insufficient AP if you click on a planet the object is already at
            int totalApCost = moveableObject.movementApCost;
            if (!ActionPointManager.Instance.CanPerformAction(totalApCost))
            {
                //Not Enough AP. Cancel.
                GUIManager.Instance.AddActionLogMessage("INSUFFICIENT AP (" + totalApCost + "): Movement Cancelled.");
                return;
            }

            //Send Move Command to moveable object
            bool moveSuccessful = moveableObject.MoveToPlanet(targetPlanet);
            if (!moveSuccessful)
            {
                //Move cancelled by moveable object.
                return;
            }

            ActionPointManager.Instance.DecreaseActionPoints(totalApCost);
        }
        
        
    }
}