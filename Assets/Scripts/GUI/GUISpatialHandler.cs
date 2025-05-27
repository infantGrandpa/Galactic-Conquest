using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUISpatialHandler : MonoBehaviour
    {
        private Transform _dynamicUITransform;

        private void Awake()
        {
            GameObject dynamicGameObject = new() {
                name = "_dynamicUI"
            };
            _dynamicUITransform = dynamicGameObject.transform;
            _dynamicUITransform.SetParent(transform);
            _dynamicUITransform.SetAsFirstSibling();
            _dynamicUITransform.localPosition = Vector3.zero;
            _dynamicUITransform.localScale = Vector3.one;
        }

        public void AddUIElement(Transform transformToAdd)
        {
            transformToAdd.SetParent(_dynamicUITransform);
            transformToAdd.localScale = Vector3.one;
        }
    }
}
