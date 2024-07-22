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

    [ReadOnly] public List<Selectable> selectedObjects = new();

    public void SelectObject()
    {
        ClearSelectedObjects();

        RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor();

        if (nullableHitInfo == null)
        {
            return;
        }

        //Convert so we can get the transform of the hit object
        RaycastHit hitInfo = (RaycastHit)nullableHitInfo;

        //Get Selectable
        Selectable hitSelectableObject = hitInfo.transform.GetComponentInParent<Selectable>();
        if (hitSelectableObject)
        {
            hitSelectableObject.SelectObject();
            return;
        }
    }

    public void ClearSelectedObjects()
    {
        List<Selectable> tempSelected = new(selectedObjects);
        foreach (Selectable thisSelectedObject in tempSelected)
        {
            thisSelectedObject.DeselectObject();
        }
    }
}
