using System;
using Abraham.GalacticConquest.Planets;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionListHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text header;
        [SerializeField] Vector2 positionOffset;

        RectTransform rectTransform;

        PlanetBehaviour currentPlanet;

        void Awake()
        {
            GetComponents();
            HideActionList();
        }

        void GetComponents()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void ShowActionList(PlanetBehaviour planet)
        {
            if (rectTransform == null) {
                GetComponents();
            }

            currentPlanet = planet;

            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(planet.transform.position, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;
            rectTransform.anchoredPosition = finalPosition;
            
            gameObject.SetActive(true);
            header.text = planet.planet.planetName;

        }

        public void HideActionList()
        {
            currentPlanet = null;
            gameObject.SetActive(false);
        }

        [ContextMenu("Recalculate")]
        public void RecalculatePosition()
        {
            if (currentPlanet == null) {
                Debug.LogWarning("No active planet.");
            }
            ShowActionList(currentPlanet);
        }
    }
}
