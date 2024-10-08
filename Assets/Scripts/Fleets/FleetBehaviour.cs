using System.Collections;
using System.Collections.Generic;
using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest.Fleets
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

        private IEnumerator CheckForBattles(PlanetBehaviour targetPlanet)
        {

            yield return CheckForSpaceBattles(targetPlanet);
            yield return new WaitForSeconds(1f);
            yield return CheckForGroundBattles(targetPlanet);

            yield return new WaitForSeconds(1f);
        }

        private IEnumerator CheckForSpaceBattles(PlanetBehaviour targetPlanet)
        {
            List<Moveable> moveablesAtPlanet = targetPlanet.PlanetSlotHandler.GetAllMoveablesAtPlanet();

            foreach (Moveable thisMoveable in moveablesAtPlanet)
            {
                IsEnemyFleet(thisMoveable, out FleetBehaviour enemyFleetBehaviour);
                if (enemyFleetBehaviour == null)
                {
                    continue;
                }

                combatBehaviour.StartSpaceBattle(enemyFleetBehaviour, targetPlanet);
                //Wait for battle to be resolved
                while (BattleManager.Instance.CurrentBattle != null)
                {
                    yield return null;
                }
            }
        }

        private IEnumerator CheckForGroundBattles(PlanetBehaviour targetPlanet)
        {
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

            combatBehaviour.StartGroundBattle(targetPlanet);
            //Wait for battle to be resolved
            while (BattleManager.Instance.CurrentBattle != null)
            {
                yield return null;
            }
        }

        private bool IsEnemyFleet(Moveable moveable, out FleetBehaviour enemyFleetBehaviour)
        {
            enemyFleetBehaviour = null;

            FactionHandler moveableFaction = moveable.GetComponent<FactionHandler>();
            if (moveableFaction == null)
            {
                //Moveable doesn't have a faction
                return false;
            }

            if (!FactionHandler.IsEnemyFaction(moveableFaction.myFaction))
            {
                //Not enemy faction
                return false;
            }

            if (!moveable.TryGetComponent(out enemyFleetBehaviour))
            {
                //not a fleet
                return false;
            }

            return enemyFleetBehaviour;
        }
    }
}
