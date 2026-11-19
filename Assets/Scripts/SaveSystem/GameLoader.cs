using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class GameLoader : MonoBehaviour
    {
        public void Load(Dictionary<string, GameObject> prototypes)
        {
            String jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, "gameObjectData.json"));
            List<ObjectData> objectDatas = JsonUtility.FromJson<List<ObjectData>>(jsonString);
            foreach (ObjectData o in objectDatas)
            {
                GameObject matchingPrototype = prototypes[o.Id];
                ISavableGameObject savableObject = matchingPrototype.GetComponent<ISavableGameObject>();
                savableObject.BuildFromObjectData(o);
            }
        }
    }
}