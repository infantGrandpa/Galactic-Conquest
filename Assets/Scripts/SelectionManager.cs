using Sirenix.OdinInspector;
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



    [SerializeField] float selectionSphereCastRadius = 0.25f;

    [ShowInInspector] private FleetBehaviour selectedFleet;


    public void SelectObject()
    {
        RaycastHit? nullableHitInfo = SphereCastFromCameraToCursor();

        if (nullableHitInfo == null)
        {
            return;
        }

        //Get Planet Behaviour
        RaycastHit hitInfo = (RaycastHit)nullableHitInfo;       //We need this to get the transform of the hit object

        //Get Fleet Behaviour
        FleetBehaviour hitFleet = hitInfo.transform.GetComponentInParent<FleetBehaviour>();
        if (hitFleet)
        {
            ChangeSelectedFleet(hitFleet);
            return;
        }
        
    }

    public void MoveToObject()
    {
        if (selectedFleet == null)
        {
            return;
        }

        RaycastHit? nullableHitInfo = SphereCastFromCameraToCursor();

        if (nullableHitInfo == null)
        {
            return;
        }

        //Get Planet Behaviour
        RaycastHit hitInfo = (RaycastHit)nullableHitInfo;       //We need this to get the transform of the hit object

        if (hitInfo.transform.TryGetComponent(out PlanetBehaviour hitPlanet))
        {
            MovementManager.Instance.MoveFleetToPlanet(selectedFleet, hitPlanet);
        }

    }

    private RaycastHit? SphereCastFromCameraToCursor()
    {
        //Sphere cast from camera to cursor position.
        Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;
        Vector3 cursorPosition = InputManager.Instance.GetCursorPosition();

        //(Desitination - Origin).normalized = direction
        Vector3 directionToCursor = (cursorPosition - cameraPosition).normalized;

        RaycastHit hitInfo;
        if (Physics.SphereCast(cameraPosition, selectionSphereCastRadius, directionToCursor, out hitInfo, 1000f))
        {
            return hitInfo;
        }

        return null;

    }

    public void ChangeSelectedFleet(FleetBehaviour newFleet)
    {
        selectedFleet = newFleet;
        Debug.Log("Changed Active Fleet to " + newFleet.gameObject.name);
    }


}
