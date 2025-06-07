using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveData
    {
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    [System.Serializable]
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
    }
}
