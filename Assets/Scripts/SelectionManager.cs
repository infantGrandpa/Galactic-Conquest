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
        //Sphere cast from camera to cursor position.
        Vector3 cameraPosition = LevelManager.Instance.MainCamera.transform.position;
        Vector3 cursorPosition = InputManager.Instance.GetCursorPosition();

        //(Desitination - Origin).normalized = direction
        Vector3 directionToCursor = (cursorPosition - cameraPosition).normalized;

        RaycastHit hitInfo;
        //TODO: Change 1000f to something less magic.
        if (Physics.SphereCast(cameraPosition, selectionSphereCastRadius, directionToCursor, out hitInfo, 1000f))
        {
            //Get Fleet Behaviour
            FleetBehaviour hitFleet = hitInfo.transform.GetComponentInParent<FleetBehaviour>();
            if (hitFleet)
            {
                ChangeSelectedFleet(hitFleet);
                return;
            }

            //Get Planet Behaviour
            if (hitInfo.transform.TryGetComponent(out PlanetBehaviour hitPlanet))
            {
                if (selectedFleet == null)
                {
                    return;
                }

                MovementManager.Instance.MoveFleetToPlanet(selectedFleet, hitPlanet);
                return;
            }
        }
    }

    public void ChangeSelectedFleet(FleetBehaviour newFleet)
    {
        selectedFleet = newFleet;
        Debug.Log("Changed Active Fleet to " + newFleet.gameObject.name);
    }


}
