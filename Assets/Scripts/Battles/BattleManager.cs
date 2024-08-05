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
        public Battle CurrentBattle { get; private set; }

        private SpaceBattleHandler spaceBattleHandler;
        private GroundBattleHandler groundBattleHandler;

        private void Awake()
        {
            spaceBattleHandler = GetComponent<SpaceBattleHandler>();
            groundBattleHandler = GetComponent<GroundBattleHandler>();
        }

        public void StartSpaceBattle(FleetBehaviour attackingFleet, FleetBehaviour defendingFleet, PlanetBehaviour planetBehaviour)
        {
            CurrentBattle = new Battle(attackingFleet, defendingFleet, planetBehaviour);
            spaceBattleHandler.StartSpaceBattle(CurrentBattle);
        }

        public void StartGroundBattle(FleetBehaviour attackingFleet, PlanetBehaviour planetBehaviour)
        {
            CurrentBattle = new Battle(attackingFleet, planetBehaviour);
            groundBattleHandler.StartGroundBattle(CurrentBattle);
        }

        public void AttackerWon()
        {
            ResolveBattle(CurrentBattle.attackingFleet);
        }

        public void DefenderWon()
        {
            ResolveBattle(CurrentBattle.defendingFleet);
        }

        private void ResolveBattle(FleetBehaviour winningFleet)
        {
            FleetBehaviour losingFleet = winningFleet == CurrentBattle.attackingFleet ? CurrentBattle.defendingFleet : CurrentBattle.attackingFleet;
            winningFleet.DamageTargetFleet(losingFleet);

            string factionName = winningFleet.FactionHandler.myFaction.FactionName;
            string planetName = CurrentBattle.battlePlanet.planetName;
            GUIManager.Instance.AddActionLogMessage(factionName + " fleet won the battle over " + planetName + "!");

            ClearBattleDetails();
        }

        private void ClearBattleDetails()
        {
            CurrentBattle = null;
        }

    }
}
