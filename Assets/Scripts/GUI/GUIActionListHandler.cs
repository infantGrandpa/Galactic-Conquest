using System;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.Traits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionListHandler : MonoBehaviour
    {
        [SerializeField] private Vector2 positionOffset;

        [Header("List Elements")] [SerializeField]
        private TMP_Text header;
        [SerializeField] private Button fortifyPlanetButton;
        [SerializeField] private Button buildFleetButton;


        private RectTransform _rectTransform;

        private PlanetBehaviour _currentPlanet;

        private void Awake()
        {
            GetComponents();
            HideActionList();
        }

        private void GetComponents()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void ShowActionList(PlanetBehaviour planetBehaviour)
        {
            if (_rectTransform == null) {
                GetComponents();
            }

            _currentPlanet = planetBehaviour;

            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(planetBehaviour.transform.position, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;
            _rectTransform.anchoredPosition = finalPosition;

            UpdateListBasedOnPlanet(planetBehaviour);
            gameObject.SetActive(true);
        }

        private void UpdateListBasedOnPlanet(PlanetBehaviour planetBehaviour)
        {
            header.text = planetBehaviour.PlanetInfo.myName;

            buildFleetButton.gameObject.SetActive(IsPlanetShipyard(planetBehaviour));

            fortifyPlanetButton.interactable = true; //TODO: Set this up to only work if planet isn't already fortified
        }

        private bool IsPlanetShipyard(PlanetBehaviour planetBehaviour)
        {
            TraitHandler traitHandler = planetBehaviour.GetComponent<TraitHandler>();
            if (traitHandler == null) {
                return false;
            }

            return traitHandler.CanBuildShips();
        }

        public void HideActionList()
        {
            _currentPlanet = null;
            gameObject.SetActive(false);
        }

        [ContextMenu("Recalculate")]
        public void RecalculatePosition()
        {
            if (_currentPlanet == null) {
                Debug.LogWarning("No active planet.");
            }
            ShowActionList(_currentPlanet);
        }

        //Called by button onclick event
        public void OnBuildFleetButtonClicked()
        {
            ShipyardBehaviour shipyardBehaviour = _currentPlanet.GetComponent<ShipyardBehaviour>();
            shipyardBehaviour.BuildFleet();

            GUIManager.Instance.AddActionLogMessage(_currentPlanet.FactionHandler.myFaction.factionName + " built a new fleet at " + _currentPlanet.PlanetInfo.myName + ".");
        }

        //Called by button onclick event
        public void OnFortifyPlanetButtonClicked()
        {
            GUIManager.Instance.AddActionLogMessage("(To Implement) Fortifying " + _currentPlanet.PlanetInfo.myName + "...");
        }
    }
}
