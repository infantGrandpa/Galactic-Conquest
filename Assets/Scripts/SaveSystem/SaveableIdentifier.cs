using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveableIdentifier : MonoBehaviour
    {
        [SerializeField] private string humanReadablePrefix;
        [SerializeField, ReadOnly] private string generatedId;
        [ShowInInspector, ReadOnly] public string FullId => BuildFullId();

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
        
        
        private string BuildFullId()
        {
            GenerateId();
            bool missingIdSection = string.IsNullOrEmpty(humanReadablePrefix) || string.IsNullOrEmpty(generatedId);
            string idSeparator  = missingIdSection ? "" : "_";
            return $"{humanReadablePrefix}{idSeparator}{generatedId}";
        }
    }
}
