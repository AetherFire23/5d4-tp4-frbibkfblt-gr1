using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    public class GameSaver
    {
        public void SaveAll(List<ISavableGameObject> saveableObjects)
        {
            List<ObjectData> objectData = new List<ObjectData>();
            foreach (ISavableGameObject saveableObject in saveableObjects)
            {
                objectData.Add(saveableObject.GetObjectData());
            } 
            String saveData = JsonUtility.ToJson(objectData, false);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "gameObjectData.json"), saveData);
        }
    }
}