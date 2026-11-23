using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class GameLoader
    {
        public List<ObjectData> GetItemsFromSave(string dirName)
        {
            return GetObjectDatasFromFile(dirName, "food_items.json");
        }

        public ObjectData GetPlayerDataFromSave(string dirName)
        {
            return GetObjectDatasFromFile(dirName, "player.json").First();
        }

        private List<ObjectData> GetObjectDatasFromFile(string dirName, string save)
        {
            String jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, dirName, save));
            AllObjectData objectDatas = JsonUtility.FromJson<AllObjectData>(jsonString); 
            return objectDatas.objectDatas;
        }

        public bool SaveExists(string dirName)
        {
            return Directory.Exists(Path.Combine(Application.persistentDataPath, dirName));
        }
    }
}