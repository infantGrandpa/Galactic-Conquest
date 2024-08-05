using System.Collections;
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

            StartCoroutine(CheckForBattles(targetPlanet));
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

        private IEnumerator CheckForBattles(PlanetBehaviour targetPlanet)
        {

            //Check for space battles
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
                //Wait for battle to be resolved
                while (BattleHandler.Instance.OngoingBattle)
                {
                    yield return null;
                }
            }

            //Check for ground battles
            if (targetPlanet.FactionHandler == null)
            {
                Debug.LogError("ERROR FleetBehaviour CheckForGroundBattle(): Planet (" + targetPlanet.gameObject.name + ") does not have a faction handler component assigned.");
                yield break;
            }

            if (!FactionHandler.IsEnemyFaction(targetPlanet.FactionHandler.myFaction))
            {
                //Not enemy faction
                yield break;
            }

            combatBehaviour.StartGroundBattle(this, targetPlanet);
            GUIManager.Instance.AddActionLogMessage("No battles at " + targetPlanet.planetName);
        }
    }
}
