using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.Refs;
using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(InputManager)) as InputManager;

                return _instance;
            }
            set => _instance = value;
        }
        private static InputManager _instance;

        [SerializeField] private float onClickSphereCastRadius = 0.25f;

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            //Left Click
            if (Input.GetMouseButtonUp(0))
            {
                // Select Planet or Fleet
                SelectionManager.Instance.SelectObject();
            }

            //Right Click
            if (Input.GetMouseButtonUp(1))
            {
                AttemptMoveToPlanet();
            }
        }

        private void AttemptMoveToPlanet()
        {
            //Check if we clicked a planet; we only move to planets.
            LayerMask planetLayerMask = LayerMaskRefs.GetLayerMask(LayerMaskRefs.PlanetLayer);
            RaycastHit? nullableHitInfo = SphereCastFromCameraToCursor(planetLayerMask);
            
            PlanetBehaviour planetToMoveTo = MovementManager.GetPlanetFromNullableHitInfo(nullableHitInfo);
            MovementManager.Instance.MoveToPlanet(planetToMoveTo);
        }

        public static Vector3 GetCursorPosition()
        {
            Ray rayFromCameraToCursor = LevelManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            Plane planetPlane = new Plane(Vector3.up, LevelManager.Instance.planetPlanePosition);
            planetPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

            return cursorPosition;
        }

        public RaycastHit? SphereCastFromCameraToCursor()
        {
            Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;
            Vector3 directionToCursor = GetDirectionToCursor(cameraPosition);

            const float maxDistance = 1000f; //TODO: Make this distance standard across the project
            if (Physics.SphereCast(cameraPosition, onClickSphereCastRadius, directionToCursor, out RaycastHit hitInfo, maxDistance))     
            {
                return hitInfo;
            }

            return null;
        }

        public RaycastHit? SphereCastFromCameraToCursor(LayerMask layerMask)
        {
            return SphereCastFromCameraToCursor(layerMask, onClickSphereCastRadius);
        }

        public RaycastHit? SphereCastFromCameraToCursor(LayerMask layerMask, float sphereCastRadius)
        {
            Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;
            Vector3 directionToCursor = GetDirectionToCursor(cameraPosition);

            if (Physics.SphereCast(cameraPosition, sphereCastRadius, directionToCursor, out RaycastHit hitInfo, 1000f, layerMask.value)) {
                return hitInfo;
            }

            return null;
        }

        private static Vector3 GetDirectionToCursor(Vector3 startPosition)
        {
            Vector3 cursorPosition = GetCursorPosition();
            Vector3 directionToCursor = (cursorPosition - startPosition).normalized;        //(Desitination - Origin).normalized = direction
            return directionToCursor;
        }
    }
}
