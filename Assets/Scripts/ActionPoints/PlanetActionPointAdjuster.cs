using System;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class PlanetActionPointAdjuster : ActionPointAdjuster
    {
        PlanetBehaviour planetBehaviour;

        void Awake()
        {
            planetBehaviour = GetComponent<PlanetBehaviour>();
            if (planetBehaviour == null) {
                Debug.LogError("ERROR PlanetActionPointAdjuster Awake(): " + gameObject.name + " is missing a Planet Behaviour component. If this isn't a planet, use the regular ActionPointAdjuster class.", this);
                return;
            }
        }

        protected override void CalculateAp()
        {
            base.CalculateAp();
            planetBehaviour.UpdateApLabel(TotalApPerTurn);
        }
    }
}
