using System;
using System.Collections;
using Abraham.GalacticConquest.GUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SelectionManager)) as SelectionManager;

                return _instance;
            }
            set => _instance = value;
        }
        private static SelectionManager _instance;

        [ReadOnly] public Selectable selectedObject;
        Coroutine manageMovementIndicatorCoroutine;

        public void SelectObject()
        {
            ClearSelectedObject();

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
                SelectObject(hitSelectableObject);
                return;
            }
            
        }

        private void SelectObject(Selectable objectToSelect)
        {
            selectedObject = objectToSelect;
            selectedObject.SelectObject();
            manageMovementIndicatorCoroutine = StartCoroutine(ManageMovementIndicator());

            GUIManager.Instance.ShowInfoBox(objectToSelect.gameObject);
        }

        private IEnumerator ManageMovementIndicator()
        {
            MovementManager.Instance.ShowMovementIndicator();
            
            while (selectedObject) {
                MovementManager.Instance.UpdateMovementIndicator();
                yield return null;
            }

            MovementManager.Instance.HideMovementIndicator();
            manageMovementIndicatorCoroutine = null;
        }

        public void ClearSelectedObject()
        {
            if (selectedObject == null)
            {
                return;
            }

            selectedObject.DeselectObject();
            selectedObject = null;

            GUIManager.Instance.HideInfoBox();
        }
    }
}