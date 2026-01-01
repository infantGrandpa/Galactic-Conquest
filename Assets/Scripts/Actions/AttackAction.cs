using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class AttackAction : GameAction
    {
        private readonly CombatantBehaviour _attacker;
        private readonly CombatantBehaviour _defender;
        private readonly PlanetBehaviour _planet;

        private readonly Faction _startingPlanetFaction;
        private readonly bool _wasPlanetFortified;

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

            _startingPlanetFaction = _planet.FactionHandler.myFaction;
            _wasPlanetFortified = _planet.IsPlanetFortified();
        }

        protected override int CalculateActionPointCost()
        {
            return ActionPointManager.Instance.attackApCost;
        }

        public override bool CanExecuteAction()
        {
            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                Result = ActionResult.Failure(GetActionTypeName(), $"Attacking requires {apCost} AP.");
                return Result.WasSuccessful;
            }
            
            return true;
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                GUIManager.Instance.AddActionLogMessage(Result.Message);
                return Result.WasSuccessful;
            }

            // If we can convert the defender into a PlanetCombatBehaviour, then this is a ground battle.
            PlanetCombatBehaviour planetCombatBehaviour = _defender as PlanetCombatBehaviour;
            Battle.BattleType battleType = planetCombatBehaviour ? Battle.BattleType.GroundBattle : Battle.BattleType.SpaceBattle;

            Battle battle = new Battle(_attacker, _defender, _planet, battleType);
            
            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);
            
            string message = GetBattleMessage(battle);
            GUIManager.Instance.AddActionLogMessage(message, apCost * -1);
            
            BattleManager.Instance.StartBattle(battle);
            
            Result = ActionResult.Success(GetActionTypeName(), apCost, message);
            return Result.WasSuccessful;
        }

        public override bool UndoAction()
        {
            _attacker.ReactivateAndCancelDeletion();    
            _defender.ReactivateAndCancelDeletion();    
            
            _planet.ChangePlanetFaction(_startingPlanetFaction);
            if (_wasPlanetFortified) _planet.FortifyPlanet();

            int apCost = Result.APCost;
            GUIManager.Instance.AddActionLogMessage($"Reverted attack at {_planet.PlanetInfo.myName}.", apCost);
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            return true;
        }

        private static string GetBattleMessage(Battle battle)
        {
            string actionLogMsg;
            switch (battle.battleType)
            {
                case Battle.BattleType.GroundBattle:
                    actionLogMsg = $"Invading {battle.battlePlanet.PlanetInfo.myName}...";
                    break;
                case Battle.BattleType.SpaceBattle:
                    actionLogMsg = $"Engaging enemy fleet over {battle.battlePlanet.PlanetInfo.myName}!";
                    break;
                default:
                    throw new ArgumentException($"Unsupported battle type: {battle.battleType}", nameof(battle.battleType));
            }

            return actionLogMsg;
        }
    }
}