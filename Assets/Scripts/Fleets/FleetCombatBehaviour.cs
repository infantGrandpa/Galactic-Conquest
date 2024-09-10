using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.Fleets
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

            GUIManager.Instance.AddActionLogMessage("Initiating space battle over " + planet.PlanetInfo.myName + "...");
            BattleManager.Instance.StartSpaceBattle(this, enemyCombatant, planet);
        }

        public void StartGroundBattle(PlanetBehaviour targetPlanet)
        {
            GUIManager.Instance.AddActionLogMessage("Invading " + targetPlanet.PlanetInfo.myName + "...");
            BattleManager.Instance.StartGroundBattle(this, targetPlanet);
        }
    }
}