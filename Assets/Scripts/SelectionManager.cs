using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(SelectionManager)) as SelectionManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static SelectionManager instance;

    [SerializeField] Vector3 planetPlanePosition = new Vector3(0, 0, 0);

    [SerializeField] GameObject testObject;

    private void Update()
    {
        //If Left Click
        if (Input.GetMouseButtonUp(0))
        {
            Ray rayFromCameraToCursor = LevelManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            Plane planetPlane = new Plane(Vector3.up, planetPlanePosition);
            planetPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

            GameObject newTestObject = Instantiate(testObject, LevelManager.Instance.DynamicTransform);
            newTestObject.transform.position = cursorPosition;
        }


    }
    private void OnDrawGizmosSelected()
    {
        DrawPlaneGizmo();
    }

    private void DrawPlaneGizmo()
    {
        /*
        I copied this code from Stop the Sniper.
        I have no idea why the planeNormal matters.
        I'm keeping it because it works.
        */
        Gizmos.color = Color.blue;

        Vector3 planeNormal = Vector3.up;

        float planeDistance = 10f;
        float shortDistance = 0f;
        Vector3 cubeSizeVector;
        if (planeNormal == Vector3.up || planeNormal == Vector3.down)
        {
            cubeSizeVector = new Vector3(planeDistance, shortDistance, planeDistance);
        }
        else if (planeNormal == Vector3.right || planeNormal == Vector3.left)
        {
            cubeSizeVector = new Vector3(shortDistance, planeDistance, planeDistance);
        }
        else
        {
            cubeSizeVector = new Vector3(planeDistance, planeDistance, shortDistance);
        }

        Gizmos.DrawWireCube(transform.position, cubeSizeVector);
    }
}
