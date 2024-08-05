using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetCombatBehaviour : CombatantBehaviour
    {
        public void StartSpaceBattle(FleetBehaviour enemyFleetBehaviour, PlanetBehaviour planet)
        {
            if (!enemyFleetBehaviour.TryGetComponent(out CombatantBehaviour enemyCombatant))
            {
                Debug.LogError("ERROR FleetCombatBehaviour StartSpaceBattle(): Enemy fleet behaviour is missing Combatant Behaviour.");
                return;
            }

            GUIManager.Instance.AddActionLogMessage("Initiating space battle over " + planet.planetName + "...");
            BattleManager.Instance.StartSpaceBattle(this, enemyCombatant, planet);
        }

        public void StartGroundBattle(PlanetBehaviour targetPlanet)
        {
            GUIManager.Instance.AddActionLogMessage("Invading " + targetPlanet.planetName + "...");
            BattleManager.Instance.StartGroundBattle(this, targetPlanet);
        }
    }
}