using System;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class PlanetActionPointAdjuster : ActionPointAdjuster
    {
        PlanetBehaviour planetBehaviour;
        
        private PlanetBehaviour GetPlanetBehaviour()
        {
            if (planetBehaviour)
            {
                return planetBehaviour;
            }

            if (TryGetComponent(out PlanetBehaviour myPlanetBehaviour))
            {
                return myPlanetBehaviour;
            }


            Debug.LogError(
                "ERROR PlanetActionPointAdjuster Awake(): " + gameObject.name +
                " is missing a Planet Behaviour component. If this isn't a planet, use the regular ActionPointAdjuster class.",
                this);
            return null;
        }

        private void UpdateAPLabel()
        {
            planetBehaviour = GetPlanetBehaviour();
            planetBehaviour?.UpdateApLabel(TotalApPerTurn);
        }

        protected override void CalculateAp()
        {
            base.CalculateAp();
            UpdateAPLabel();
        }
    }
}