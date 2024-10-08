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

        [SerializeField] float onClickSphereCastRadius = 0.25f;

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
                //Move to planet
                MovementManager.Instance.MoveToPlanet();
            }
        }

        public Vector3 GetCursorPosition()
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

            RaycastHit hitInfo;
            if (Physics.SphereCast(cameraPosition, onClickSphereCastRadius, directionToCursor, out hitInfo, 1000f))     //TODO: Make distance not magic
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

        private Vector3 GetDirectionToCursor(Vector3 startPosition)
        {
            Vector3 cursorPosition = GetCursorPosition();
            Vector3 directionToCursor = (cursorPosition - startPosition).normalized;        //(Desitination - Origin).normalized = direction
            return directionToCursor;
        }
    }
}
