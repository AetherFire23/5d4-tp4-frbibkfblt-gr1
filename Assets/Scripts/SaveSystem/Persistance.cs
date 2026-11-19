using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SaveSystem
{
    public class Persistance
    {
        private GameSaver _gameSaver;
        private GameLoader _gameLoader;
        private List<ObjectData> uniques = new List<ObjectData>();
        private Dictionary<string, AbstractPrototypeFactory> prototypeIdToPrototypeFactory = new();
        private Dictionary<string, List<ObjectData>> prototypeIdToPrototypeData = new();

        public static Persistance Instance = new(
            new GameLoader(),
            new GameSaver()
        );

        private Persistance(GameLoader gameLoader, GameSaver gameSaver)
        {
            _gameLoader = gameLoader;
            _gameSaver = gameSaver;
        }

        public static void RegisterUnique(IUniqueObject go)
        {
            if (go == null) return;
            Instance.uniques.Add(go.GetObjectData());
        }
        
        public static void RegisterPrototypeFactory(AbstractPrototypeFactory abstractPrototypeFactory)
        {
            Instance.prototypeIdToPrototypeFactory.Add(abstractPrototypeFactory.GetProtoId(), abstractPrototypeFactory);
        }
        
        public static void RegisterPrototype(ISavableGameObject go)
        {
            if (!Instance.prototypeIdToPrototypeData.ContainsKey(go.GetObjectData().Id))
                Instance.prototypeIdToPrototypeData.Add(go.GetObjectData().Id, new List<ObjectData>());
            
            Instance.prototypeIdToPrototypeData[go.GetObjectData().Id].Add(go.GetObjectData());
        }

        public static void Save()
        {
            Debug.Log("Saved game");
            Instance._gameSaver.SaveAll(Instance.prototypeIdToPrototypeData.Values.Flatten(s ).ToList());
        }

        public static void Load()
        {
            // todo fetch ObjectData
            
            foreach (KeyValuePair<string, ObjectData> idToProto in Instance.prototypeIdToPrototypeData)
            {
                AbstractPrototypeFactory abstractPrototypeFactory = Instance.prototypeIdToPrototypeFactory[idToProto.Key]; // get factory for prototype
                abstractPrototypeFactory.Create(idToProto.Value); // generate
            }
        }

        public static bool SaveExists()
        {
            return Instance._gameLoader.SaveExists();
        }


    }
}