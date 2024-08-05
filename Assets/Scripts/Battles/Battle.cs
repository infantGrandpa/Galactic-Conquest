using System;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    [System.Serializable]
    public class Battle
    {
        public enum BattleType
        {
            SpaceBattle,
            GroundBattle
        }

        public CombatantBehaviour attacker;
        public CombatantBehaviour defender;
        public PlanetBehaviour battlePlanet;
        public BattleType battleType;

        public Battle(CombatantBehaviour attacker, CombatantBehaviour defender, PlanetBehaviour battlePlanet, BattleType battleType)
        {
            this.attacker = attacker;
            this.defender = defender;
            this.battlePlanet = battlePlanet;
            this.battleType = battleType;
        }
    }
}
