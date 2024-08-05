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

        public void StartSpaceBattle(CombatantBehaviour attacker, CombatantBehaviour defender, PlanetBehaviour planet)
        {
            CurrentBattle = new Battle(attacker, defender, planet, Battle.BattleType.SpaceBattle);
            spaceBattleHandler.StartSpaceBattle(CurrentBattle);
        }

        public void StartGroundBattle(CombatantBehaviour attacker, PlanetBehaviour planet)
        {
            if (!planet.TryGetComponent(out CombatantBehaviour defender))
            {
                Debug.LogError("ERROR BattleManger StartGroundBattle(): Planet is missing a CombatantBehaviour and cannot participate in a ground battle.", this);
                return;
            }
            CurrentBattle = new Battle(attacker, defender, planet, Battle.BattleType.GroundBattle);
            groundBattleHandler.StartGroundBattle(CurrentBattle);
        }

        public void AttackerWon()
        {
            ResolveBattle(CurrentBattle.attacker);
        }

        public void DefenderWon()
        {
            ResolveBattle(CurrentBattle.defender);
        }

        private void ResolveBattle(CombatantBehaviour winner)
        {
            CombatantBehaviour loser = winner == CurrentBattle.attacker ? CurrentBattle.defender : CurrentBattle.attacker;

            if (CurrentBattle.battleType == Battle.BattleType.GroundBattle && loser is PlanetCombatBehaviour)
            {
                PlanetCombatBehaviour loserPlanet = (PlanetCombatBehaviour)loser;
                loserPlanet.PrepareForInvasion(winner);
            }

            winner.DamageTarget(loser);
            PrintWinMessage(winner);
            ClearBattleDetails();
        }

        private void PrintWinMessage(CombatantBehaviour winner)
        {
            if (!winner.TryGetComponent(out FactionHandler winningFactionHandler))
            {
                Debug.LogWarning("BattleManager ResolveBattle(): Winner (" + winner.gameObject.name + ") does not have a FactionHandler component.");
            }
            string factionName = winningFactionHandler.myFaction.FactionName;
            string planetName = CurrentBattle.battlePlanet.planetName;
            GUIManager.Instance.AddActionLogMessage("The " + factionName + " won the battle at " + planetName + "!");
        }

        private void ClearBattleDetails()
        {
            CurrentBattle = null;
        }

    }
}
