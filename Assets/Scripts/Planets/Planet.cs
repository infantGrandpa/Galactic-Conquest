using Abraham.GalacticConquest.Factions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.Planets
{
    [System.Serializable]
    public class Planet
    {
        public string planetName;
        public PlanetSpecialty planetSpecialty;
        public bool isShipyard;

        public Planet(string planetName, PlanetSpecialty planetSpecialty, bool isShipyard)
        {
            this.planetName = planetName;
            this.planetSpecialty = planetSpecialty;
            this.isShipyard = isShipyard;
        }

        public override string ToString()
        {
            return planetName;
        }
    }

    public enum PlanetSpecialty
    {
        None,
        Capital,
        ProductionCenter
    }
}
