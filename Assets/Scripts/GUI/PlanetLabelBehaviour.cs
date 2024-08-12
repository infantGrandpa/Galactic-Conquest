using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using UnityEngine;
using TMPro;

namespace Abraham.GalacticConquest
{
    public class PlanetLabelBehaviour : MonoBehaviour
    {
        [SerializeField] TMP_Text planetNameText;
        Faction currentFaction;
        RectTransform rectTransform;

        [SerializeField] Vector2 positionOffset;

        Vector3 currentWorldPosition;

        public void InitLabel(string planetName, Faction faction, Vector3 worldPosition)
        {
            currentFaction = faction;
            planetNameText.text = planetName;

            GUIManager.Instance.AddUIElementToSpatialCanvas(transform);

            rectTransform = GetComponent<RectTransform>();

            SetCanvasPosition(worldPosition);

        }
        public void SetCanvasPosition(Vector3 worldPosition)
        {
            currentWorldPosition = worldPosition;
            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(worldPosition, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;

            rectTransform.anchoredPosition = finalPosition;
        }

        [ContextMenu("Recalc Canvas Position")]
        private void RecalcCanvasPosition()
        {
            SetCanvasPosition(currentWorldPosition);
        }
    }
}
