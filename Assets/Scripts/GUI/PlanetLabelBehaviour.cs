using System;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.Planets;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Abraham.GalacticConquest.GUI
{
    public class PlanetLabelBehaviour : MonoBehaviour
    {
        [SerializeField] TMP_Text planetNameText;
        Faction currentFaction;
        RectTransform rectTransform;

        [SerializeField] Vector2 positionOffset;

        Vector3 currentWorldPosition;

        [SerializeField] Image capitalImage;
        [SerializeField] Image prodCenterImage;
        [SerializeField] Image shipyardImage;

        [SerializeField] TMP_Text apLabel;

        public void InitLabel(Planet planetDetails, Faction faction, Vector3 worldPosition)
        {
            currentFaction = faction;
            planetNameText.text = planetDetails.planetName;

            ShowSpecialtyImages(planetDetails.planetSpecialty, planetDetails.isShipyard);

            GUIManager.Instance.AddUIElementToSpatialCanvas(transform);

            rectTransform = GetComponent<RectTransform>();

            SetColors(faction.factionColor);
            SetCanvasPosition(worldPosition);
        }

        public void UpdateAPLabel(int newAP)
        {
            apLabel.text = newAP + " AP";
        }

        private void ShowSpecialtyImages(PlanetSpecialty planetSpecialty, bool isShipyard)
        {
            GameObject capitalGameObject = capitalImage.gameObject;
            GameObject prodCenterGameObject = prodCenterImage.gameObject;
            GameObject shipyardGameObject = shipyardImage.gameObject;

            //Hide everything
            capitalGameObject.SetActive(false);
            prodCenterGameObject.SetActive(false);
            shipyardGameObject.SetActive(false);

            if (isShipyard) {
                shipyardGameObject.SetActive(true);
            }

            //Show speciality icons
            switch (planetSpecialty) {
            case PlanetSpecialty.Capital:
                capitalGameObject.SetActive(true);
                break;
            case PlanetSpecialty.ProductionCenter:
                prodCenterGameObject.SetActive(true);
                break;
            default:
                break;
            }

        }

        private void SetColors(Color newColor)
        {
            capitalImage.color = newColor;
            prodCenterImage.color = newColor;
            shipyardImage.color = newColor;

            planetNameText.color = newColor;
            apLabel.color = newColor;
        }

        void SetCanvasPosition(Vector3 worldPosition)
        {
            currentWorldPosition = worldPosition;
            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(worldPosition, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;

            rectTransform.anchoredPosition = finalPosition;
        }

        public void UpdateLabelFaction(Faction newFaction)
        {
            currentFaction = newFaction;

            SetColors(currentFaction.factionColor);
        }
    }
}
