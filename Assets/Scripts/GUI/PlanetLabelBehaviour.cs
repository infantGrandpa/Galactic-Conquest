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
        [SerializeField] private TMP_Text planetNameText;
        private Faction _currentFaction;
        private RectTransform _rectTransform;

        [SerializeField] private Vector2 positionOffset;

        private Vector3 _currentWorldPosition;

        [SerializeField] private List<Image> traitIcons = new List<Image>();
        
        [SerializeField] private TMP_Text apLabel;

        public void InitLabel(GenericInfo planetDetails, Faction faction, TraitHandler traitHandler, Vector3 worldPosition)
        {
            _currentFaction = faction;
            planetNameText.text = planetDetails.myName;

            ShowTraitIcons(traitHandler);

            GUIManager.Instance.AddUIElementToSpatialCanvas(transform);

            _rectTransform = GetComponent<RectTransform>();

            SetColors(faction.factionColor);
            SetCanvasPosition(worldPosition);
        }

        public void UpdateAPLabel(int newAP)
        {
            apLabel.text = newAP + " AP";
        }

        public void ShowTraitIcons(TraitHandler traitHandler)
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

        private void SetCanvasPosition(Vector3 worldPosition)
        {
            _currentWorldPosition = worldPosition;
            Vector2 canvasPosition = GUIManager.Instance.mainCanvas.WorldToCanvasPosition(worldPosition, GUIManager.Instance.mainCamera);
            Vector2 finalPosition = canvasPosition + positionOffset;

            _rectTransform.anchoredPosition = finalPosition;
        }

        public void UpdateLabelFaction(Faction newFaction)
        {
            _currentFaction = newFaction;

            SetColors(_currentFaction.factionColor);
        }
    }
}
