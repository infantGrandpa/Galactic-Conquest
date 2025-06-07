using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SaveSystem)) as SaveSystem;

                return _instance;
            }
            set { _instance = value; }
        }

        private static SaveSystem _instance;

        [ContextMenu("Save Single Data")]
        public void TestSingleSaveData()
        {
            List<SaveData> allSaveData = CollectSaveData();
            SaveData saveData = allSaveData[0];

            string json = saveData.ToJson();

            string filePath = GetSaveFilePath("testsave");

            SaveJsonToFile(json, filePath);
        }

        private List<SaveData> CollectSaveData()
        {
            IEnumerable<ISaveable> allSaveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
            List<SaveData> allSaveData = new List<SaveData>();

            foreach (ISaveable thisSaveableObject in allSaveableObjects)
            {
                allSaveData.Add(thisSaveableObject.SerializeToSaveData());
            }

            return allSaveData;
        }

        private void SaveJsonToFile(string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }

        private string GetSaveFilePath(string saveFileName)
        {
            // This saves to Unity's persistent data path, which is:
            // - Windows: %userprofile%\AppData\LocalLow\<companyname>\<productname>
            // - Mac: ~/Library/Application Support/<companyname>/<productname>
            // - Linux: ~/.config/unity3d/<companyname>/<productname>

            string fileName = $"{saveFileName}.json";
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        [ContextMenu("Open Save Folder")]
        public void OpenSaveFolder()
        {
            Application.OpenURL("file://" + Application.persistentDataPath);
        }
    }
}