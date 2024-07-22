using System.Collections.Generic;
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

    [SerializeField] private GameObject selectionObjectPrefab;
    private GameObject selectionRing;

    [ReadOnly] public List<Selectable> selectedObjects = new();

    private void Start()
    {
        
    }

    public void SelectObject()
    {
        RaycastHit? nullableHitInfo = SphereCastFromCameraToCursor();

        if (nullableHitInfo == null)
        {
            return;
        }

        //Convert so we can get the transform of the hit object
        RaycastHit hitInfo = (RaycastHit)nullableHitInfo;       

        //Get Selectable
        Selectable hitSelectableObject = hitInfo.transform.GetComponentInParent<Selectable>();
        if (hitSelectableObject) {
            hitSelectableObject.SelectObject();
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
        HandleSelectionRingChange();
    }

    private void HandleSelectionRingChange()
    {
        if (selectedFleet == null)
        {
            selectionRing.SetActive(false);
            return;
        }

        selectionRing.SetActive(true);
        selectionRing.transform.position = selectedFleet.transform.position;
        selectionRing.transform.SetParent(selectedFleet.transform);
    }
}
