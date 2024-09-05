using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class MovementIndicatorHandler : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;

        public void ShowLineRenderer()
        {
            lineRenderer.gameObject.SetActive(true);
        }
        
        public void HideLineRenderer()
        {
            lineRenderer.gameObject.SetActive(false);
        }
        
        public void SetMovementLinePositions(Vector3 startPosition, Vector3 endPosition)
        {
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
