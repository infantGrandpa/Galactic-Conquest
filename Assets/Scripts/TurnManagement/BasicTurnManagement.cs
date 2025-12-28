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
        // TODO: Replace the factionsInGame list with the ActiveFactionManager
        [SerializeField] private List<Faction> factionsInGame = new();
        public Faction currentFactionTurn;


        private void Start()
        {
            //If currentFactionTurn is set, then we loaded a game.
            // This means AP should already be set, and we shouldn't recalculate.
            //  This has to happen after we've set currentFactionTurn on Awake() using EasySave3.
            if (currentFactionTurn)
            {
                SetCurrentTurn(currentFactionTurn, false);
                GUIManager.Instance.UpdateActionPoints(ActionPointManager.Instance.currentActionPoints);
                return;
            }
            
            SetCurrentTurn(startingFaction);
        }

        public void NextTurn()
        {
            ActionPointManager.Instance.SaveRolloverPoints(currentFactionTurn);
            GarbageCollector.Instance.ClearGarbage();
            
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

        private void SetCurrentTurn(Faction faction, bool calculateAp = true)
        {
            currentFactionTurn = faction;
            GUIManager.Instance.ChangeTurn($"{currentFactionTurn.factionName}'s Turn");
            if (!calculateAp)
            {
                return;
            }
            ActionPointManager.Instance.CalculateActionPoints(currentFactionTurn);
        }
    }
}