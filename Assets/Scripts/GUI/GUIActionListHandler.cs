using Abraham.GalacticConquest.Actions;
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
        [SerializeField] private Button invadePlanetButton;

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
            
            //TODO: Set this up to only work if planet isn't already fortified
            fortifyPlanetButton.interactable = true; 

            invadePlanetButton.gameObject.SetActive(CanInvadePlanet(planetBehaviour));
        }

        private static bool IsPlanetShipyard(PlanetBehaviour planetBehaviour)
        {
            TraitHandler traitHandler = planetBehaviour.GetComponent<TraitHandler>();
            if (traitHandler == null) {
                return false;
            }

            return traitHandler.CanBuildShips();
        }

        private bool CanInvadePlanet(PlanetBehaviour planetBehaviour)
        {
            GameObject enemyAtPlanet = planetBehaviour.GetEnemyAtPlanet(planetBehaviour.FactionHandler.myFaction);
            return (bool)enemyAtPlanet;
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
            BuildFleetAction buildFleetAction = new BuildFleetAction(shipyardBehaviour);
            ActionManager.Instance.PerformAction(buildFleetAction);
        }

        //Called by button onclick event
        public void OnFortifyPlanetButtonClicked()
        {
            GUIManager.Instance.AddActionLogMessage("(To Implement) Fortifying " + _currentPlanet.PlanetInfo.myName + "...");
        }
        
        //Called by button onclick event
        public void OnInvadePlanetButtonClicked()
        {
            GUIManager.Instance.AddActionLogMessage("(To Implement) Invading " + _currentPlanet.PlanetInfo.myName + "...");
        }
    }
}
