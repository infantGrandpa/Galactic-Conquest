using UnityEngine;
using System.Collections.Generic;

public class PlanetBehaviour : MonoBehaviour
{
    [SerializeField] List<Transform> fleetPositions = new();
    [SerializeField] List<FleetBehaviour> fleetsAtThisPlanet = new();

    public bool AddFleetToPlanet(FleetBehaviour fleetBehaviour) {
        
    }
}
