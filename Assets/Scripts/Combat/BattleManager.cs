using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.Combat
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(BattleManager)) as BattleManager;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static BattleManager _instance;
        private Battle _currentBattle;

        public void StartBattle(Battle battle)
        {
            _currentBattle = battle;
            GUIManager.Instance.ShowBattleDialogBox(battle);
        }

        public void AttackerWon()
        {
            ResolveBattle(_currentBattle.attacker);
        }

        public void DefenderWon()
        {
            ResolveBattle(_currentBattle.defender);
        }

        private void ResolveBattle(CombatantBehaviour winner)
        {
            CombatantBehaviour loser = winner == _currentBattle.attacker ? _currentBattle.defender : _currentBattle.attacker;

            if (_currentBattle.battleType == Battle.BattleType.GroundBattle && loser is PlanetCombatBehaviour)
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
                throw new MissingComponentException($"Winner ({winner.gameObject.name}) does not have a FactionHandler component.");
            }
            string factionName = winningFactionHandler.myFaction.factionName;
            string planetName = _currentBattle.battlePlanet.PlanetInfo.myName;
            GUIManager.Instance.AddActionLogMessage("The " + factionName + " won the battle at " + planetName + "!");
        }

        private void ClearBattleDetails()
        {
            _currentBattle = null;
        }

    }
}
