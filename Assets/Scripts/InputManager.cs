using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(InputManager)) as InputManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static InputManager instance;

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
            //Move to fleet or planet
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
        //Sphere cast from camera to cursor position.
        Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;
        Vector3 cursorPosition = InputManager.Instance.GetCursorPosition();

        //(Desitination - Origin).normalized = direction
        Vector3 directionToCursor = (cursorPosition - cameraPosition).normalized;

        RaycastHit hitInfo;
        if (Physics.SphereCast(cameraPosition, onClickSphereCastRadius, directionToCursor, out hitInfo, 1000f))
        {
            return hitInfo;
        }

        return null;
    }
}
