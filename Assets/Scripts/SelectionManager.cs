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

    [SerializeField] float selectionSphereCastRadius = 0.25f;

    private void Update()
    {
        //If Left Click
        if (Input.GetMouseButtonUp(0))
        {

            Ray rayFromCameraToCursor = LevelManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            Plane planetPlane = new Plane(Vector3.up, planetPlanePosition);
            planetPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

            //Sphere cast from camera to cursor position.
            Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;

            //(Desitination - Origin).normalized = direction
            Vector3 directionToCursor = (cursorPosition - cameraPosition).normalized;

            RaycastHit hitInfo;
            //TODO: Change 1000f to something less magic.
            if (Physics.SphereCast(cameraPosition, selectionSphereCastRadius, directionToCursor, out hitInfo, 1000f))
            {
                //Get Planet Behaviour
                if (hitInfo.transform.TryGetComponent(out PlanetBehaviour hitPlanet))
                {
                    MovementManager.Instance.MoveActiveFleetToPlanet(hitPlanet);
                }
            }

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
