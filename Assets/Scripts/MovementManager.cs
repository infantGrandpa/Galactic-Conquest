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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlanets();
        }
    }

    private void SwitchPlanets(int planetIndex = -1)
    {
        //Get planet behaivour
        PlanetBehaviour targetPlanet = GetNextPlanet(targetFleet.currentPlanet);

        if (targetPlanet == null)
        {
            Debug.LogError("ERROR: Target Planet is null.");
            return;
        }

        targetFleet.MoveFleetToPlanetPosition(targetPlanet);
    }

    private PlanetBehaviour GetNextPlanet(PlanetBehaviour currentPlanet)
    {
        int nextPlanetIndex = 0;
        for (int i = 0; i < planets.Count; i++)
        {
            if (currentPlanet != planets[i])
            {
                continue;
            }

            nextPlanetIndex = i + 1;
            if (nextPlanetIndex >= planets.Count)
            {
                nextPlanetIndex = 0;
            }
        }


        return planets[nextPlanetIndex];
    }

    public void MoveActiveFleetToPlanet(PlanetBehaviour targetPlanet)
    {
        targetFleet.MoveFleetToPlanetPosition(targetPlanet);
    }
}
