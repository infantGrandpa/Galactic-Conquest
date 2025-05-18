using System;
using Abraham.GalacticConquest.ActionPoints;
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
            set { _instance = value; }
        }

        private static MovementManager _instance;

        [Header("Move Line Indicator")]
        [SerializeField] private GameObject movementIndicatorLinePrefab;
        private MovementIndicatorHandler movementIndicatorHandler;
        private Vector3 _selectedMoveablePosition;

        [SerializeField] private float movementIndicatorSphereCastRadius;

        [Header("Movement Rings")]
        [SerializeField] private int movementRings = 5;
        [SerializeField] private float movementRingRadius = 12.5f;
        private int _activeMovementRing;

        //TEMP
        [SerializeField] private Color[] ringColors;
        [SerializeField] private bool showAllRings = false;
        //END TEMP

        private void Awake()
        {
            if (movementIndicatorLinePrefab == null)
            {
                Debug.LogError("ERROR MovementManager Awake(): Movement Indicator prefab is null.", this);
                return;
            }

            GameObject movementIndicatorObject =
                Instantiate(movementIndicatorLinePrefab, LevelManager.Instance.DynamicTransform);
            movementIndicatorHandler = movementIndicatorObject.GetComponent<MovementIndicatorHandler>();

            if (movementIndicatorHandler == null)
            {
                Debug.LogError(
                    "ERROR MovementManager Awake(): Provided Movement Indicator Line Prefab does not have a MovementIndicatorHandler component.",
                    this);
                return;
            }

            movementIndicatorHandler.HideLineRenderer();
        }

        private Moveable GetMoveableFromSelectedObject()
        {
            //Cancel if nothing is selected
            Selectable selectedObject = SelectionManager.Instance.selectedObject;
            if (selectedObject == null)
            {
                return null;
            }

            //Is Object Moveable
            if (!selectedObject.TryGetComponent(out Moveable moveableObject))
            {
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
            return GetPlanetFromNullableHitInfo(nullableHitInfo);
        }

        private PlanetBehaviour GetPlanetFromNullableHitInfo(RaycastHit? nullableHitInfo)
        {
            if (nullableHitInfo == null)
            {
                //Nothing hit
                return null;
            }

            //Get target planet
            RaycastHit
                hitInfo = (RaycastHit)nullableHitInfo; //Convert hit info so we can get the transform of the hit object
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            return targetPlanet;
        }

        public void MoveToPlanet()
        {
            Moveable moveableObject = GetMoveableFromSelectedObject();
            if (moveableObject == null)
            {
                return;
            }

            PlanetBehaviour targetPlanet = GetPlanetToMoveTo();
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

            // Get Ring Level
            float distanceToTarget = moveableObject.GetDistanceToTarget(targetPlanet.transform.position);
            _activeMovementRing = GetRingLevelFromDistance(distanceToTarget);
            
            //Check AP costs. 
            // This is last so we don't send a message about insufficient AP if you click on a planet the object is already at
            int totalApCost = moveableObject.CalculateMovementCost(_activeMovementRing);
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
            if (moveableObject == null)
            {
                return;
            }

            //Get Closest Planet
            LayerMask planetLayerMask = LayerMaskRefs.GetLayerMask(LayerMaskRefs.PlanetLayer);
            RaycastHit? nullableHitInfo =
                InputManager.Instance.SphereCastFromCameraToCursor(planetLayerMask, movementIndicatorSphereCastRadius);

            PlanetBehaviour targetPlanet = GetPlanetFromNullableHitInfo(nullableHitInfo);

            _selectedMoveablePosition = moveableObject.transform.position;
            Vector3 endPosition = targetPlanet == null
                ? InputManager.Instance.GetCursorPosition()
                : targetPlanet.transform.position;
            
            _activeMovementRing = GetRingLevelFromDistance(_selectedMoveablePosition, endPosition);
            int apCost = moveableObject.CalculateMovementCost(_activeMovementRing);
            
            GUIManager.Instance.UpdateMovementCostIndicator(apCost);

            movementIndicatorHandler.SetMovementLinePositions(_selectedMoveablePosition, endPosition);
        }

        public void HideMovementIndicator()
        {
            movementIndicatorHandler.HideLineRenderer();
        }

        public void ShowMovementIndicator()
        {
            movementIndicatorHandler.ShowLineRenderer();
        }

        private int GetRingLevelFromDistance(float distanceToTarget)
        {
            int ringLevel = Mathf.CeilToInt(distanceToTarget / movementRingRadius);
            return Mathf.Clamp(ringLevel, 1, 5);
        }

        private int GetRingLevelFromDistance(Vector3 startPosition, Vector3 endPosition)
        {
            float distance = Vector3.Distance(startPosition, endPosition);
            return GetRingLevelFromDistance(distance);
        }


        private void OnDrawGizmos()
        {
            if (ringColors == null || ringColors.Length < movementRings)
            {
                Debug.LogWarning("Not enough colors defined for movement rings.");
                return;
            }

            if (showAllRings)
            {
                for (int i = 0; i < movementRings; i++)
                {
                    Gizmos.color = ringColors[i];
                    Gizmos.DrawWireSphere(_selectedMoveablePosition, movementRingRadius * (i + 1));
                }
            }
            else if (_activeMovementRing > 0 && _activeMovementRing <= movementRings)
            {
                int ringIndex = _activeMovementRing - 1;
                Gizmos.color = ringColors[ringIndex];
                Gizmos.DrawWireSphere(_selectedMoveablePosition, movementRingRadius * _activeMovementRing);
            }
        }
    }
}