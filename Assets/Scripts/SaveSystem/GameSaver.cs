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
        public void SaveAll(List<ObjectData> saveableObjects)
        {
            AllObjectData datum = new AllObjectData();
            datum.SetObjectDatas(saveableObjects);
            string saveData = JsonConvert.SerializeObject(datum, Formatting.None, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });            
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.json"), saveData);
        }
    }
}