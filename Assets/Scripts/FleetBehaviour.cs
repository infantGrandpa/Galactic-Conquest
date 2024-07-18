using UnityEngine;

public class FleetBehaviour : MonoBehaviour
{
    public void MoveFleetToPlanetPosition(PlanetBehaviour targetPlanet)
    {
        Transform planetTransform = targetPlanet.transform;
        Debug.Log("Moving fleet to " + targetPlanet.gameObject.name + " at " + planetTransform.position);

        transform.position = planetTransform.position;
        return;
    }
}
