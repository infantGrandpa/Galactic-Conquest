using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
{
    [System.Serializable]
    public class SelectableInfo
    {
        public string infoBoxTitle;
        public string infoBoxDesc;
        [ReadOnly] public int apPerTurn;
    }
}
