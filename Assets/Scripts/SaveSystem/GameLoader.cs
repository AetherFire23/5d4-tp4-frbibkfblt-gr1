using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class GameLoader
    {
        public void Load(Dictionary<string, GameObject> prototypes)
        {
            String jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.json"));
            AllObjectData objectDatas = JsonUtility.FromJson<AllObjectData>(jsonString);
            foreach (ObjectData o in objectDatas.objectDatas)
            {
                GameObject matchingPrototype = prototypes[o.Id];
                ISavableGameObject savableObject = matchingPrototype.GetComponent<ISavableGameObject>();
                savableObject.BuildFromObjectData(o);
            }
        }

        public bool SaveExists()
        {
            return File.Exists(Path.Combine(Application.persistentDataPath, "save.json"));
        }
    }
}