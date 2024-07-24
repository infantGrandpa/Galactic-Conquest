using UnityEngine;

namespace Abraham.GalacticConquest
{
    [CreateAssetMenu(fileName = "Faction", menuName = "Game/Faction")]
    public class Faction : ScriptableObject
    {
        public string FactionName;
        public Color FactionColor;
    }
}