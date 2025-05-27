using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest.Fleets
{
    public class FleetMoveable : Moveable
    {
        private FleetBehaviour _fleetBehaviour;

        private void Awake()
        {
            _fleetBehaviour = GetComponent<FleetBehaviour>();
        }

        public override bool MoveToPlanet(PlanetBehaviour targetPlanet)
        {
            bool moveSuccessful = base.MoveToPlanet(targetPlanet);

            if (!moveSuccessful)
            {
                return false;
            }

            if (_fleetBehaviour == null)
            {
                Debug.LogError("ERROR FleetMoveable MoveToPlanet: Fleet Behaviour is null.", this);
                return false;
            }

            _fleetBehaviour.FleetArrivedAtPlanet(targetPlanet);

            return true;
        }
    }
}
