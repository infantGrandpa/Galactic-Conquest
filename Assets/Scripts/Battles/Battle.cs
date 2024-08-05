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

        public FleetBehaviour attackingFleet;
        public FleetBehaviour defendingFleet;
        public PlanetBehaviour battlePlanet;
        public BattleType battleType;

        // Constructor for ground battles
        public Battle(FleetBehaviour attackingFleet, PlanetBehaviour planetBehaviour)
        {
            this.attackingFleet = attackingFleet;
            this.battlePlanet = planetBehaviour;
            this.battleType = BattleType.GroundBattle;
        }

        // Constructor for space battles
        public Battle(FleetBehaviour attackingFleet, FleetBehaviour defendingFleet, PlanetBehaviour planetBehaviour)
        {
            this.attackingFleet = attackingFleet;
            this.defendingFleet = defendingFleet;
            this.battlePlanet = planetBehaviour;
            this.battleType = BattleType.SpaceBattle;
        }
    }
}
