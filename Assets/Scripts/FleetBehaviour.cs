using UnityEngine;

public class FleetBehaviour : MonoBehaviour
{
    public PlanetBehaviour currentPlanet;

    public void MoveFleetToPlanetPosition(PlanetBehaviour targetPlanet)
    {
        currentPlanet = targetPlanet;
        transform.position = targetPlanet.transform.position;
    }
}
