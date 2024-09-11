using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.TurnManagement;
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
            TurnStateMachine.Instance.AddFactionHandlerToTurnList(this);
        }

        void OnDisable()
        {
            TurnStateMachine.Instance?.RemoveFactionHandlerFromTurnList(this);
        }

        private void Start()
        {
            SetFaction(myFaction);
        }

        public void SetFaction(Faction newFaction)
        {
            myFaction = newFaction;
            foreach (Renderer thisRenderer in renderersToChangeOnSetFaction)
            {
                thisRenderer.material.color = myFaction.factionColor;
            }

            foreach (Image thisImage in uiImagesToChangeOnSetFaction)
            {
                thisImage.color = myFaction.factionColor;
            }
        }

        public bool IsEnemyFaction(Faction faction)
        {
            if (faction == myFaction)
            {
                return false;
            }

            return true;
        }
    }
}