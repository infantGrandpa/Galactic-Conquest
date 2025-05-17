using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.TurnManagement
{
    public class BasicTurnManagement : MonoBehaviour
    {
        public static BasicTurnManagement Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(BasicTurnManagement)) as BasicTurnManagement;

                return _instance;
            }
            set => _instance = value;
        }
        private static BasicTurnManagement _instance;
        
        //current faction's turn variable
        [SerializeField] private Faction startingFaction;
        [SerializeField] private List<Faction> factionsInGame = new List<Faction>();
        private Faction currentFactionTurn;


        private void Start()
        {
            SetCurrentTurn(startingFaction);
        }

        public void NextTurn()
        {
            int currentIndex = factionsInGame.IndexOf(currentFactionTurn);
    
            // Safety check
            if (currentIndex == -1 || factionsInGame.Count == 0)
            {
                Debug.LogWarning("Current faction not found in list or list is empty!");
                return;
            }

            int nextIndex = (currentIndex + 1) % factionsInGame.Count;
            SetCurrentTurn(factionsInGame[nextIndex]);
        }

        private void SetCurrentTurn(Faction faction)
        {
            currentFactionTurn = faction;
            GUIManager.Instance.ChangeTurn($"{currentFactionTurn.factionName}'s Turn");
            ActionPointManager.Instance.CalculateActionPoints();
        }

        //TODO: add list of rollover action points

        //Move to next faction in active factions
        //Recalculate AP
        // Show whose turn it currently is in UI
    }
}