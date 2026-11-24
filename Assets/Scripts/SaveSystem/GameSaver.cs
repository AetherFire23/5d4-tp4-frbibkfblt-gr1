using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class GameSaver
    {
        public void SavePlayer(ObjectData objectData, string saveDir)
        {
            List<ObjectData> objectDatas = new List<ObjectData> { objectData };
            SaveAll(objectDatas, saveDir, "player.json");
        }
        
        public void SaveFoodItems(List<ObjectData> objects, string saveDir)
        {
            SaveAll(objects, saveDir, "food_items.json");
        }
        
        private void SaveAll(List<ObjectData> saveableObjects, string saveDir, string saveName)
        {
            AllObjectData datum = new AllObjectData();
            datum.SetObjectDatas(saveableObjects);
            string saveData = JsonConvert.SerializeObject(datum, Formatting.None, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });            
            
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, saveDir)))
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, saveDir));
            File.WriteAllText(Path.Combine(Application.persistentDataPath, saveDir, saveName), saveData);
        }
    }
}