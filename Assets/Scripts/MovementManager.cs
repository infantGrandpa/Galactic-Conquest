using UnityEngine;
using System.Collections.Generic;
using System;

public class MovementManager : MonoBehaviour
{
    //Singleton
    public static MovementManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(MovementManager)) as MovementManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static MovementManager instance;

    public List<PlanetBehaviour> planets = new();

    public void MoveFleetToPlanet(FleetBehaviour fleetToMove, PlanetBehaviour targetPlanet)
    {
        if (fleetToMove == null)
        {
            Debug.LogWarning("MovementManager MoveFleetToPlanet(): Cannot move to " + targetPlanet.gameObject.name);
            return;
        }

        fleetToMove.MoveFleetToPlanetPosition(targetPlanet);
    }
}
