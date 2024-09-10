using System;
using Abraham.GalacticConquest.Planets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class ActionPointGenerator : MonoBehaviour
    {
        [ReadOnly] public int APGeneratedPerTurn { get; private set; }

        Planet planetInfo;

        void OnEnable()
        {
            ActionPointManager.Instance.actionPointGenerators.Add(this);
        }

        void OnDisable()
        {
            if (ActionPointManager.Instance == null) {
                return;
            }
            ActionPointManager.Instance.actionPointGenerators.Remove(this);
        }

        void Start()
        {
            PlanetBehaviour planetBehaviour = GetPlanetInfo();
            APGeneratedPerTurn = ActionPointManager.Instance.GetAPGeneratorValue(planetInfo);
        }
        PlanetBehaviour GetPlanetInfo()
        {
            PlanetBehaviour planetBehaviour = GetComponent<PlanetBehaviour>();
            if (planetBehaviour == null) {
                Debug.LogError("ERROR ActionPointGenerator Start(): " + gameObject.name + " is missing a PlanetBehaviour component.", this);
                return planetBehaviour;
            }

            planetInfo = planetBehaviour.planet;
            return planetBehaviour;
        }
    }
}
