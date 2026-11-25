using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Knight;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class GameLoader
    {
        public List<ObjectData> GetItemsFromSave(string dirName)
        {
            return GetObjectDatasFromFile<ObjectData>(dirName, "food_items.json");
        }

        public ObjectData GetPlayerDataFromSave(string dirName)
        {
            return GetObjectDatasFromFile<ObjectData>(dirName, "player.json").First();
        }

        public KnightRegionSave GetKnightRegionSave(string dirname)
        {
            KnightRegionSave obj = (KnightRegionSave)GetObjectDatasFromFile<KnightRegionSave>(dirname, "knight_status.json").First();
            return obj;
        }

        private List<ObjectData> GetObjectDatasFromFile<T>(string dirName, string save) where T: ObjectData
        {
            String jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, dirName, save));
            AllObjectData<T> objectDatas = JsonConvert.DeserializeObject<AllObjectData<T>>(jsonString);
            return new List<ObjectData>(objectDatas.objectDatas);
        }

        // private KnightRegionSave GetKnightRegionSave(string dirName, string save)
        // {
        //     String jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, dirName, save));
        //     KnightRegionSave objectDatas = JsonUtility.FromJson<KnightRegionSave>(jsonString);
        //     return objectDatas;
        // }

        public bool SaveExists(string dirName)
        {
            return Directory.Exists(Path.Combine(Application.persistentDataPath, dirName));
        }
    }
}