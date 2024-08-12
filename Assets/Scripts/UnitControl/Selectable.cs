using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
{
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
                SelectionManager.Instance.ClearSelectedObject();
            }
        }

        public void SelectObject()
        {
            if (mySelectionRing == null)
            {
                Debug.LogError("ERROR Selectable SelectObject(): Selection Ring not assigned.");
                return;
            }

            mySelectionRing.SetActive(true);
        }

        public void DeselectObject()
        {
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

            if (SelectionManager.Instance.selectedObject == this)
            {
                return true;
            }

            return false;
        }
    }
}