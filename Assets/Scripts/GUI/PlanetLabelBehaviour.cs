using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.Traits;
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

        [SerializeField] List<Image> traitIcons = new List<Image>();
        
        [SerializeField] TMP_Text apLabel;

        public void InitLabel(Planet planetDetails, Faction faction, TraitHandler traitHandler, Vector3 worldPosition)
        {
            currentFaction = faction;
            planetNameText.text = planetDetails.planetName;

            ShowTraitIcons(traitHandler);

            GUIManager.Instance.AddUIElementToSpatialCanvas(transform);

            rectTransform = GetComponent<RectTransform>();

            SetColors(faction.factionColor);
            SetCanvasPosition(worldPosition);
        }

        public void UpdateAPLabel(int newAP)
        {
            apLabel.text = newAP + " AP";
        }

        void ShowTraitIcons(TraitHandler traitHandler)
        {
            foreach (Image thisTraitIconSlot in traitIcons) {
                thisTraitIconSlot.gameObject.SetActive(false);
            }

            int traitCount = 0;
            foreach (Trait thisTrait in traitHandler.traits) {
                if (thisTrait.traitIcon == null) {
                    continue;
                }

                Image thisTraitIconSlot = traitIcons[traitCount];
                thisTraitIconSlot.sprite = thisTrait.traitIcon;
                thisTraitIconSlot.gameObject.SetActive(true);

                traitCount++;
            }
        }

        private void SetColors(Color newColor)
        {
            foreach (Image thisTraitIconSlot in traitIcons) {
                thisTraitIconSlot.color = newColor;
            }

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
