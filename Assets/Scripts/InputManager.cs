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
            SelectionManager.Instance.MoveToObject();
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
}
