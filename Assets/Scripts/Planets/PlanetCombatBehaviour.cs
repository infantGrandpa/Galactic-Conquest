using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlanetCombatBehaviour : CombatantBehaviour
    {
        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        public void CapturePlanet()
        {
            GUIManager.Instance.AddActionLogMessage("Planet captured.");
            healthSystem.HealFully();
        }
    }
}
