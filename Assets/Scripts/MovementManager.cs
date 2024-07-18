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
    public FleetBehaviour targetFleet;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SwitchPlanets();
        }
    }

    private void SwitchPlanets(int planetIndex = -1)
    {
        //Get planet behaivour
        PlanetBehaviour targetPlanet = planets[0];

        targetFleet.MoveFleetToPlanetPosition(targetPlanet);
    }
}
