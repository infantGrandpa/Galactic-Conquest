using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetBehaviour : MonoBehaviour
    {
        public FactionHandler FactionHandler { get; private set; }

        public IDamageable HealthSystem { get; private set; }

        private FleetCombatBehaviour combatBehaviour;

        private void Awake()
        {
            FactionHandler = GetComponent<FactionHandler>();
            HealthSystem = GetComponent<IDamageable>();
            combatBehaviour = GetComponent<FleetCombatBehaviour>();
        }

        public void FleetArrivedAtPlanet(PlanetBehaviour targetPlanet)
        {
            if (targetPlanet == null)
            {
                Debug.LogError("ERROR FleetBehaviour FleetArrivedAtPlanet(): Fleet (" + gameObject.name + ") cannot arrive at planet because the provided planet is null.");
                return;
            }

            bool spaceBattleStarted = CheckForSpaceBattle(targetPlanet);

            if (spaceBattleStarted)
            {
                return;
            }

            CheckForGroundBattle(targetPlanet);
        }

        private bool CheckForSpaceBattle(PlanetBehaviour targetPlanet)
        {
            List<Moveable> moveablesAtPlanet = targetPlanet.PlanetSlotHandler.GetAllMoveablesAtPlanet();

            foreach (Moveable thisMoveable in moveablesAtPlanet)
            {
                FactionHandler moveableFaction = thisMoveable.GetComponent<FactionHandler>();
                if (moveableFaction == null)
                {
                    //Moveable doesn't have a faction
                    continue;
                }

                if (!FactionHandler.IsEnemyFaction(moveableFaction.myFaction))
                {
                    //Not enemy faction
                    continue;
                }

                if (!thisMoveable.TryGetComponent(out FleetBehaviour enemyFleetBehaviour))
                {
                    //not a fleet
                    continue;
                }

                combatBehaviour.StartSpaceBattle(this, enemyFleetBehaviour, targetPlanet);
                return true;        //This means only 1 space battle can be started at a time. Not sure if that'll ever be an issue.
            }

            //No space battle
            return false;
        }

        private bool CheckForGroundBattle(PlanetBehaviour targetPlanet)
        {
            if (targetPlanet.FactionHandler == null)
            {
                Debug.LogError("ERROR FleetBehaviour CheckForGroundBattle(): Planet (" + targetPlanet.gameObject.name + ") does not have a faction handler component assigned.");
                return false;
            }

            if (!FactionHandler.IsEnemyFaction(targetPlanet.FactionHandler.myFaction))
            {
                //Not enemy faction
                return false;
            }

            combatBehaviour.StartGroundBattle(this, targetPlanet);
            return true;
        }

        public void DamageTargetFleet(FleetBehaviour targetFleet)
        {
            if (combatBehaviour == null)
            {
                Debug.LogError("ERROR FleetBehaviour DamageTargetFleet(): This fleet (" + gameObject.name + ") does not have a FleetCombatBehaviour component.", this);
                return;
            }
            combatBehaviour.DamageTarget(targetFleet);
        }
    }
}
