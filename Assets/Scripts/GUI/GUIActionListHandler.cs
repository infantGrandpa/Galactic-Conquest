using System;
using Abraham.GalacticConquest.Planets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionListHandler : MonoBehaviour
    {
        [SerializeField] Vector2 positionOffset;

        [Header("List Elements")] [SerializeField]
        TMP_Text header;
        [SerializeField] Button fortifyPlanetButton;
        [SerializeField] Button buildFleetButton;
        

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

        public void ShowActionList(PlanetBehaviour planetBehaviour)
        {
            if (rectTransform == null) {
                GetComponents();
            }

            currentPlanet = planetBehaviour;

            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(planetBehaviour.transform.position, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;
            rectTransform.anchoredPosition = finalPosition;

            UpdateListBasedOnPlanet(planetBehaviour.planet);
            gameObject.SetActive(true);
        }

        private void UpdateListBasedOnPlanet(Planet planet)
        {
            header.text = planet.planetName;

            buildFleetButton.gameObject.SetActive(planet.isShipyard);

            fortifyPlanetButton.interactable = true; //TODO: Set this up to only work if planet isn't already fortified
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

        //Called by button onclick event
        public void OnBuildFleetButtonClicked()
        {
            GUIManager.Instance.AddActionLogMessage("Building a new fleet at " + currentPlanet.planet.planetName + "...");
        }

        //Called by button onclick event
        public void OnFortifyPlanetButtonClicked()
        {
            GUIManager.Instance.AddActionLogMessage("Fortifying " + currentPlanet.planet.planetName + "...");
        }
    }
}
