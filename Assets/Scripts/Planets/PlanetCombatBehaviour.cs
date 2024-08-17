using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.Planets
{
    public class PlanetCombatBehaviour : CombatantBehaviour
    {
        private HealthSystem healthSystem;
        private CombatantBehaviour invader;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        public void ResetPlanetAfterCapture()
        {
            healthSystem.HealFully();
            invader = null;
        }

        public void PrepareForInvasion(CombatantBehaviour newInvader)
        {
            this.invader = newInvader;
        }

        public Faction GetInvaderFaction()
        {
            if (invader == null)
            {
                Debug.LogError("ERROR PlanetCombatBehaviour GetInvaderFaction(): Invader is null.", this);
                return null;
            }

            if (!invader.TryGetComponent(out FactionHandler invaderFactionHandler))
            {
                Debug.LogError("ERROR PlanetCombatBehaviour GetInvaderFaction(): Invader doesn't have a FactionHandler component.", this);
                return null;
            }

            return invaderFactionHandler.myFaction;
        }
    }
}
