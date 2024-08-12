using UnityEngine;

namespace Abraham.GalacticConquest.GUI
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
            dynamicUITransform.localScale = Vector3.one;
        }

        public void AddUIElement(Transform transformToAdd)
        {
            transformToAdd.SetParent(dynamicUITransform);
            transformToAdd.localScale = Vector3.one;
        }
    }
}
