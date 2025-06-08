using Abraham.GalacticConquest.Planets;
using UnityEngine;
using System;

namespace Abraham.GalacticConquest.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string testField = "test";
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
            
        }
    }

    [Serializable]
    public class PlanetData : SaveData
    {
        public string planetName;
        public string typeDescriptor;
        public Vector3 worldPosition;

        public PlanetData(string jsonData)
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
        }
        
        public PlanetData(PlanetBehaviour planetBehaviour)
        {
            planetName = planetBehaviour.PlanetInfo.myName;
            typeDescriptor = planetBehaviour.PlanetInfo.typeDescriptor;
            worldPosition = planetBehaviour.transform.position;
        }

        public override string ToString()
        {
            return $"{typeDescriptor} {planetName} {worldPosition.ToString()}";
        }
    }
}
