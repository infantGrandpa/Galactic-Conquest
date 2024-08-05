using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlanetCombatBehaviour : CombatantBehaviour
    {
        private HealthSystem healthSystem;
        private CombatantBehaviour invader;
        private FactionHandler factionHandler;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            factionHandler = GetComponent<FactionHandler>();
        }

        //TODO: Determine if capturing the planet should all happen in the PlanetBehaviour component
        public void CapturePlanet()
        {
            Faction newFaction = GetInvaderFaction();
            factionHandler.SetFaction(newFaction);

            GUIManager.Instance.AddActionLogMessage("Planet captured by " + newFaction.FactionName + "!");
            healthSystem.HealFully();

            invader = null;
        }

        public void PrepareForInvasion(CombatantBehaviour invader)
        {
            this.invader = invader;
        }

        private Faction GetInvaderFaction()
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
