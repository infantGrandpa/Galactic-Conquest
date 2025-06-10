using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    [ExecuteInEditMode]
    public class SaveableIdentifier : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string generatedId;

        private void Awake()
        {
            GenerateId();            
        }

        private void GenerateId()
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            if (assetType != PrefabAssetType.NotAPrefab)
            {
                generatedId = null;
                return;
            }
            
            if (string.IsNullOrEmpty(generatedId))
            {
                generatedId = System.Guid.NewGuid().ToString();
            }
        }

        public string GetGeneratedId()
        {
            GenerateId();
            return generatedId;
        }
    }
}
