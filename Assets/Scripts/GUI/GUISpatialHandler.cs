using System;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GUISpatialHandler : MonoBehaviour
    {
        Transform dynamicUITransform;

        void Awake()
        {
            GameObject dynamicGameObject = new() {
                name = "_dynamicUI"
            };
            dynamicUITransform = dynamicGameObject.transform;
            dynamicUITransform.SetParent(transform);
            dynamicUITransform.localPosition = Vector3.zero;
        }

        public void AddUIElement(Transform transformToAdd)
        {
            transformToAdd.SetParent(dynamicUITransform);
        }
    }
}
