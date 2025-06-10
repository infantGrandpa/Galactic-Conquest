using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveableIdentifier : MonoBehaviour
    {
        [ReadOnly] public string uniqueID;
        
        [Button("Generate ID", ButtonSizes.Gigantic, DirtyOnClick = true), GUIColor(0, 1, 0)]
        private void GenerateId()
        {
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
}
