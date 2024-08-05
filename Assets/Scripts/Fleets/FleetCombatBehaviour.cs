using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetCombatBehaviour : MonoBehaviour
    {
        public int damageToDeal;

        public void DamageTarget(IDamageable damageableTarget)
        {
            if (damageableTarget == null)
            {
                Debug.LogError("ERROR FleetCombatBehaviour DamageTarget(): Provided damageableTarget is null.");
                return;
            }

            damageableTarget.TakeDamage(damageToDeal);
        }

        public void DamageTarget(FleetBehaviour targetFleet)
        {
            if (targetFleet == null)
            {
                Debug.LogError("ERROR FleetCombatBehaviour DamageTarget(): Target Fleet is null.");
                return;
            }

            if (targetFleet.HealthSystem == null)
            {
                Debug.LogError("ERROR FleetCombatBehaviour DamageTarget(): Target Fleet (" + targetFleet.gameObject.name + ") is missing an IDamagable component.");
                return;
            }

            DamageTarget(targetFleet.HealthSystem);
        }

        public void StartSpaceBattle(FleetBehaviour myFleetBehaviour, FleetBehaviour enemyFleetBehaviour, PlanetBehaviour planet)
        {
            GUIManager.Instance.AddActionLogMessage("Initiating space battle over " + planet.planetName + "...");
            BattleHandler.Instance.StartBattle(myFleetBehaviour, enemyFleetBehaviour, planet);
        }
    }
}