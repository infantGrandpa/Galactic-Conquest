using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
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
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(MovementManager)) as MovementManager;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static MovementManager _instance;

        [SerializeField] GameObject movementIndicatorLinePrefab;
        MovementIndicatorHandler movementIndicatorHandler;

        void Awake()
        {
            if (movementIndicatorLinePrefab == null) {
                Debug.LogError("ERROR MovementManager Awake(): Movement Indicator prefab is null.", this);
                return;
            }

            GameObject movementIndicatorObject = Instantiate(movementIndicatorLinePrefab, LevelManager.Instance.DynamicTransform);
            movementIndicatorHandler = movementIndicatorObject.GetComponent<MovementIndicatorHandler>();

            if (movementIndicatorHandler == null) {
                Debug.LogError("ERROR MovementManager Awake(): Provided Movement Indicator Line Prefab does not have a MovementIndicatorHandler component.", this);
                return;
            }
        }

        private Moveable GetMoveableFromSelectedObject()
        {
            //Cancel if nothing is selected
            Selectable selectedObject = SelectionManager.Instance.selectedObject;
            if (selectedObject == null) {
                return null;
            }

            //Is Object Moveable
            if (!selectedObject.TryGetComponent(out Moveable moveableObject)) {
                //Object not movable. Cancel.
                return null;
            }

            return moveableObject;
        }

        private PlanetBehaviour GetPlanetToMoveTo()
        {
            //Get Move To Target
            LayerMask planetLayerMask = LayerMaskRefs.GetLayerMask(LayerMaskRefs.PlanetLayer);
            RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor(planetLayerMask);
            if (nullableHitInfo == null) {
                //Didn't click on anything. Cancel.
                return null;
            }

            //Get target planet
            RaycastHit hitInfo = (RaycastHit)nullableHitInfo; //Convert hit info so we can get the transform of the hit object
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            return targetPlanet;
        }
        
        public void MoveToPlanet()
        {
            Moveable moveableObject = GetMoveableFromSelectedObject();
            if (moveableObject == null) {
                return;
            }

            PlanetBehaviour targetPlanet = GetPlanetToMoveTo();
            if (!targetPlanet) {
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
            int totalApCost = moveableObject.CalculateMovementCost(targetPlanet);
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


        public void UpdateMovementIndicator()
        {
            Moveable moveableObject = GetMoveableFromSelectedObject();
            if (moveableObject == null) {
                return;
            }

            Vector3 startPosition = moveableObject.transform.position;
            Vector3 endPosition = InputManager.Instance.GetCursorPosition();

            movementIndicatorHandler.SetMovementLinePositions(startPosition, endPosition);

        }



        
        
    }
}