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

        // Constructor for ground battles
        public Battle(CombatantBehaviour attacker, PlanetBehaviour planet)
        {
            this.attacker = attacker;
            this.battlePlanet = planet;
            this.battleType = BattleType.GroundBattle;
        }

        // Constructor for space battles
        public Battle(CombatantBehaviour attacker, CombatantBehaviour defender, PlanetBehaviour planet)
        {
            this.attacker = attacker;
            this.defender = defender;
            this.battlePlanet = planet;
            this.battleType = BattleType.SpaceBattle;
        }
    }
}
