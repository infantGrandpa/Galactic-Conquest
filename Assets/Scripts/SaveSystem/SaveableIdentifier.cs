using Sirenix.OdinInspector;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveableIdentifier : MonoBehaviour
    {
        [SerializeField] private string customIdPrefix;
        private string _generatedId;
        [ShowInInspector, ReadOnly] public string FullId => BuildFullId();

        private void Awake()
        {
            GenerateId();            
        }

        private void GenerateId()
        {
            if (string.IsNullOrEmpty(_generatedId))
            {
                _generatedId = System.Guid.NewGuid().ToString();
            }
        }

        private string BuildFullId()
        {
            string id = "";
            if (!string.IsNullOrEmpty(customIdPrefix))
            {
                id += $"{customIdPrefix}_";
            }
            
            GenerateId();

            id += _generatedId;
            return id;
        }
    }
}
