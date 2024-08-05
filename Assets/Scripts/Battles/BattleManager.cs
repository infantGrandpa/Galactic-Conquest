using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(BattleManager)) as BattleManager;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static BattleManager instance;
        public bool OngoingBattle { get; private set; }
        public Battle currentBattle { get; private set; }

        private SpaceBattleHandler spaceBattleHandler;

        private void Awake()
        {
            OngoingBattle = false;
            spaceBattleHandler = GetComponent<SpaceBattleHandler>();

        }

        public void StartSpaceBattle(FleetBehaviour attackingFleet, FleetBehaviour defendingFleet, PlanetBehaviour planetBehaviour)
        {
            currentBattle = new Battle(attackingFleet, defendingFleet, planetBehaviour);

            OngoingBattle = true;
            spaceBattleHandler.StartSpaceBattle(currentBattle);
        }

        public void AttackerWon()
        {
            ResolveBattle(currentBattle.attackingFleet);
        }

        public void DefenderWon()
        {
            ResolveBattle(currentBattle.defendingFleet);
        }

        private void ResolveBattle(FleetBehaviour winningFleet)
        {
            FleetBehaviour losingFleet = winningFleet == currentBattle.attackingFleet ? currentBattle.defendingFleet : currentBattle.attackingFleet;
            winningFleet.DamageTargetFleet(losingFleet);

            string factionName = winningFleet.FactionHandler.myFaction.FactionName;
            string planetName = currentBattle.battlePlanet.planetName;
            GUIManager.Instance.AddActionLogMessage(factionName + " fleet won the battle over " + planetName + "!");

            ClearBattleDetails();
        }

        private void ClearBattleDetails()
        {
            currentBattle = null;
            OngoingBattle = false;
        }

    }
}
