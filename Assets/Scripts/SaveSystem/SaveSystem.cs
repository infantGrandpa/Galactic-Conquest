using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        public List<SaveData> allSaveData = new();

        public SaveDataCollection saveDataCollection;

        [ContextMenu("Save Single Data")]
        public void TestSingleSaveData()
        {
            allSaveData = CollectSaveData();
            SaveData saveData = allSaveData[0];

            string json = JsonUtility.ToJson(saveData, true);

            string filePath = GetSaveFilePath("testsave-single");

            SaveJsonToFile(json, filePath);
        }

        [ContextMenu("Save All Data")]
        public void TestMultipleSaveData()
        {
            allSaveData = CollectSaveData();
            saveDataCollection = new SaveDataCollection();
            saveDataCollection.saveDataList = allSaveData;

            string json = JsonUtility.ToJson(saveDataCollection, true);
            Debug.Log("json = " + json);

            string saveFileFullPath = GetSaveFilePath("testsave-all");

            SaveJsonToFile(json, saveFileFullPath);
        }

        private List<SaveData> CollectSaveData()
        {
            IEnumerable<ISaveable> allSaveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
                
            List<SaveData> saveDataList = new List<SaveData>();

            foreach (ISaveable thisSaveableObject in allSaveableObjects)
            {
                saveDataList.Add(thisSaveableObject.SerializeToSaveData());
            }

            return saveDataList;
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
    
    [Serializable]
    public class SaveDataCollection
    {
        public List<SaveData> saveDataList;
    }
}