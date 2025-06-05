using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class AttackAction : IGameAction
    {
        private readonly CombatantBehaviour _attacker;
        private readonly CombatantBehaviour _defender;
        private readonly PlanetBehaviour _planet;

        public AttackAction(CombatantBehaviour attacker, CombatantBehaviour defender, PlanetBehaviour planet)
        {
            _attacker = attacker;
            _defender = defender;
            _planet = planet;
        }

        public AttackAction(GameObject attackerObject, GameObject defenderObject, PlanetBehaviour planet)
        {
            if (!attackerObject.TryGetComponent(out CombatantBehaviour attackerCombatantBehaviour))
            {
                throw new MissingComponentException($"Attacker ({attackerObject.name}) is missing a Combatant behaviour.");
            }

            if (!defenderObject.TryGetComponent(out CombatantBehaviour defenderCombatantBehaviour))
            {
                throw new MissingComponentException($"Defender ({defenderObject.name}) is missing a Combatant behaviour.");
            }

            _attacker = attackerCombatantBehaviour;
            _defender = defenderCombatantBehaviour;
            _planet = planet;
        }
        
        public int GetActionPointCost()
        {
            return ActionPointManager.Instance.attackApCost;
        }

        public bool CanExecuteAction()
        {
            int apCost = GetActionPointCost();
            return ActionPointManager.Instance.CanPerformAction(apCost);
        }

        public bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return false;
            }
            
            // If we can convert the defender into a PlanetCombatBehaviour, then this is a ground battle.
            PlanetCombatBehaviour planetCombatBehaviour = _defender as PlanetCombatBehaviour;
            Battle.BattleType battleType = planetCombatBehaviour ? Battle.BattleType.GroundBattle : Battle.BattleType.SpaceBattle;
            
            Battle battle = new Battle(_attacker, _defender, _planet, battleType);
            LogBattle(battle);
            
            BattleManager.Instance.StartBattle(battle);
            ActionPointManager.Instance.DecreaseActionPoints(GetActionPointCost());
            return true;
        }

        public bool UndoAction()
        {
            throw new NotImplementedException();
        }

        private void LogBattle(Battle battle)
        {
            string actionLogMsg;
            switch (battle.battleType)
            {
                case Battle.BattleType.GroundBattle:
                    actionLogMsg = $"Invading {battle.battlePlanet.PlanetInfo.myName}...";
                    break;
                case Battle.BattleType.SpaceBattle:
                    actionLogMsg = $"Engaging enemy forces over {battle.battlePlanet.PlanetInfo.myName}!";
                    break;
                default:
                    throw new ArgumentException($"Unsupported battle type: {battle.battleType}", nameof(battle.battleType));
            }
            GUIManager.Instance.AddActionLogMessage(actionLogMsg, GetActionPointCost() * -1);
        }
    }
}
