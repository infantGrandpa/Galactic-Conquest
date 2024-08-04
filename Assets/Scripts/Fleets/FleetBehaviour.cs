using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetBehaviour : MonoBehaviour
    {
        public bool IsPlanetHostile(PlanetBehaviour targetPlanet)
        {
            GUIManager.Instance.AddActionLogMessage("Arrived at " + targetPlanet.planetName + " planet.");
            return false;
        }
    }
}
