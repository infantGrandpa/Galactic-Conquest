using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.TurnManagement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Abraham.GalacticConquest.Factions
{
    public class FactionHandler : MonoBehaviour
    {
        public Faction myFaction;

        [SerializeField] List<Renderer> renderersToChangeOnSetFaction = new();
        [SerializeField] List<Image> uiImagesToChangeOnSetFaction = new();

        void OnEnable()
        {
            ActiveFactionManager.Instance.AddFactionHandlerToTurnList(this);
        }

        void OnDisable()
        {
            ActiveFactionManager.Instance?.RemoveFactionHandlerFromTurnList(this);
        }

        private void Start()
        {
            SetFaction(myFaction);
        }

        public void SetFaction(Faction newFaction)
        {
            Faction oldFaction = myFaction;
            myFaction = newFaction;
            UpdateAppearanceToMatchFaction();

            ActiveFactionManager.Instance.IsFactionActive(oldFaction);
        }

        public bool IsEnemyFaction(Faction faction)
        {
            if (faction == myFaction)
            {
                return false;
            }

            return true;
        }

        [Button("Update Colors to Match Faction")]
        private void UpdateAppearanceToMatchFaction()
        {
            foreach (Renderer thisRenderer in renderersToChangeOnSetFaction)
            {
                thisRenderer.material.color = myFaction.factionColor;
            }

            foreach (Image thisImage in uiImagesToChangeOnSetFaction)
            {
                thisImage.color = myFaction.factionColor;
            }
        }
    }
}