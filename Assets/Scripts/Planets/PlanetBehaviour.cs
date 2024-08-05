using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public string planetName;

        public PlanetSlotHandler PlanetSlotHandler { get; private set; }
        public PlanetCombatBehaviour PlanetCombatBehaviour { get; private set; }
        public FactionHandler FactionHandler { get; private set; }

        private void Awake()
        {
            PlanetSlotHandler = GetComponent<PlanetSlotHandler>();
            PlanetCombatBehaviour = GetComponent<PlanetCombatBehaviour>();
            FactionHandler = GetComponent<FactionHandler>();
        }


    }
}
