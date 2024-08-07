using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest
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
                selectedObject = hitSelectableObject;
                selectedObject.SelectObject();
                return;
            }
        }

        public void ClearSelectedObject()
        {
            if (selectedObject == null)
            {
                return;
            }

            selectedObject.DeselectObject();
            selectedObject = null;
        }
    }
}