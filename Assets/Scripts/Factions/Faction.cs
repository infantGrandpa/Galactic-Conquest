using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.Factions
{
    [CreateAssetMenu(fileName = "Faction", menuName = "Game/Faction")]
    public class Faction : ScriptableObject
    {
        [FormerlySerializedAs("FactionName")] public string factionName;
        [FormerlySerializedAs("FactionColor")] public Color factionColor;
    }
}