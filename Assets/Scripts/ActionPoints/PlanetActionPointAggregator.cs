using System;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class PlanetActionPointAggregator : ActionPointAggregator
    {
        private PlanetBehaviour _planetBehaviour;
        
        private PlanetBehaviour GetPlanetBehaviour()
        {
            if (_planetBehaviour)
            {
                return _planetBehaviour;
            }

            if (TryGetComponent(out PlanetBehaviour myPlanetBehaviour))
            {
                return myPlanetBehaviour;
            }


            Debug.LogError(
                "ERROR PlanetActionPointModifier Awake(): " + gameObject.name +
                " is missing a Planet Behaviour component. If this isn't a planet, use the regular ActionPointAggregator class.",
                this);
            return null;
        }

        private void UpdateAPLabel()
        {
            _planetBehaviour = GetPlanetBehaviour();
            _planetBehaviour?.UpdateApLabel(TotalApPerTurn);
        }

        protected override void CalculateAp()
        {
            base.CalculateAp();
            UpdateAPLabel();
        }
    }
}