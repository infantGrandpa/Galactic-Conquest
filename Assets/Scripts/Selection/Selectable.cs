using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] GameObject mySelectionRing;

    private void OnEnable()
    {
        if (mySelectionRing == null)
        {
            Debug.LogError("ERROR Selectable OnEnable(): Selection Ring not assigned.");
            return;
        }

        mySelectionRing.SetActive(false);
    }

    private void OnDisable()
    {
        if (SelectionManager.Instance == null)
        {
            return;
        }

        if (IsSelected())
        {
            SelectionManager.Instance.selectedObjects.Remove(this);
        }
    }

    public void SelectObject()
    {
        SelectionManager.Instance.selectedObjects.Add(this);

        if (mySelectionRing == null)
        {
            Debug.LogError("ERROR Selectable SelectObject(): Selection Ring not assigned.");
            return;
        }

        mySelectionRing.SetActive(true);

    }

    public void DeselectObject()
    {
        SelectionManager.Instance.selectedObjects.Remove(this);

        if (mySelectionRing == null)
        {
            Debug.LogError("ERROR Selectable SelectObject(): Selection Ring not assigned.");
            return;
        }

        mySelectionRing.SetActive(false);
    }

    private bool IsSelected()
    {
        if (SelectionManager.Instance == null)
        {
            return false;
        }

        List<Selectable> selectedObjects = SelectionManager.Instance.selectedObjects;
        foreach (Selectable thisSelectedObject in selectedObjects)
        {
            if (thisSelectedObject == this)
            {
                return true;
            }
        }


        return false;
    }
}
