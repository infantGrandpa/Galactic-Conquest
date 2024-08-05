using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class BattleHandler : MonoBehaviour
    {
        public static BattleHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(BattleHandler)) as BattleHandler;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static BattleHandler instance;

        private FleetBehaviour attackingFleet;
        private FleetBehaviour defendingFleet;

        private PlanetBehaviour battlePlanet;

        public void StartBattle(FleetBehaviour newAttackingFleet, FleetBehaviour newDefendingFleet, PlanetBehaviour planetBehaviour)
        {
            attackingFleet = newAttackingFleet;
            defendingFleet = newDefendingFleet;
            battlePlanet = planetBehaviour;
            GUIManager.Instance.ShowBattleDialogBox(attackingFleet.FactionHandler.myFaction, defendingFleet.FactionHandler.myFaction, battlePlanet);
        }

        public void AttackerWon()
        {
            ResolveBattle(attackingFleet);
        }

        public void DefenderWon()
        {
            ResolveBattle(defendingFleet);
        }

        private void ResolveBattle(FleetBehaviour winningFleet)
        {
            string factionName = winningFleet.FactionHandler.myFaction.FactionName;

            FleetBehaviour losingFleet = winningFleet == attackingFleet ? defendingFleet : attackingFleet;
            winningFleet.DamageTargetFleet(losingFleet);

            GUIManager.Instance.AddActionLogMessage(factionName + " fleet won the battle over " + battlePlanet.planetName + "!");
            ClearBattleDetails();
        }

        private void ClearBattleDetails()
        {
            attackingFleet = null;
            defendingFleet = null;
            battlePlanet = null;
        }
    }
}
