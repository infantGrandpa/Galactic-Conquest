using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetMoveable : Moveable
    {
        FleetBehaviour fleetBehaviour;

        private void Awake()
        {
            fleetBehaviour = GetComponent<FleetBehaviour>();
        }

        public override bool MoveToPlanet(PlanetBehaviour targetPlanet)
        {
            bool moveSuccessful = base.MoveToPlanet(targetPlanet);

            if (!moveSuccessful)
            {
                return false;
            }

            if (fleetBehaviour == null)
            {
                Debug.LogError("ERROR FleetMoveable MoveToPlanet: Fleet Behaviour is null.", this);
                return false;
            }

            fleetBehaviour.FleetArrivedAtPlanet(targetPlanet);

            return true;
        }
    }
}
