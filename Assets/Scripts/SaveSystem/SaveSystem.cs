using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Abraham.GalacticConquest.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        public SaveDataCollection saveDataCollection;
        public string saveFileName = "test-save";

        [ContextMenu("Save All Data")]
        public void SaveGameData()
        {
            saveDataCollection = new SaveDataCollection
            {
                saveDataList = CollectSaveData()
            };

            string json = JsonUtility.ToJson(saveDataCollection, true);
            string saveFilePath = GetSaveFilePath(saveFileName);
            SaveJsonToFile(json, saveFilePath);
        }

        private static List<SaveData> CollectSaveData()
        {
            IEnumerable<ISaveable> allSaveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

            List<SaveData> saveDataList = new List<SaveData>();

            foreach (ISaveable thisSaveableObject in allSaveableObjects)
            {
                saveDataList.Add(thisSaveableObject.SerializeToSaveData());
            }

            return saveDataList;
        }

        private static void SaveJsonToFile(string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }

        private static string GetSaveFilePath(string fileName)
        {
            // This saves to Unity's persistent data path, which is:
            // - Windows: %userprofile%\AppData\LocalLow\<companyname>\<productname>
            // - Mac: ~/Library/Application Support/<companyname>/<productname>
            // - Linux: ~/.config/unity3d/<companyname>/<productname>

            string fileNameWithExt = $"{fileName}.json";
            return Path.Combine(Application.persistentDataPath, fileNameWithExt);
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
        /*
         * SerializeReference is required for "polymorphism", aka including both a parent and it's children.
         * Since we need SaveData and all it's children's data, we need to tell the serializer that this could be SaveData, PlanetData, etc.
         */
        [SerializeReference] public List<SaveData> saveDataList;
    }
}