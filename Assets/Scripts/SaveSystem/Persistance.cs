using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace SaveSystem
{
    public class Persistance
    {
        private GameSaver _gameSaver;
        private GameLoader _gameLoader;
        private Dictionary<string, GameObject> prototypes = new Dictionary<string, GameObject>();
        private List<ISavableGameObject> savableObjects = new List<ISavableGameObject>();
        
        public static Persistance Instance = new(
            new GameLoader(),
            new GameSaver()
            );

        private Persistance(GameLoader gameLoader, GameSaver gameSaver)
        {
            _gameLoader = gameLoader;
            _gameSaver = gameSaver;
        }

        public static void Register(ISavableGameObject go)
        {
            if (go == null) return;
            
            Instance.savableObjects.Add(go);
            if (!Instance.prototypes.ContainsKey(go.GetObjectData().Id))
                Instance.prototypes.Add(go.GetObjectData().Id, go.GetGameObject());
        }

        public static void Save()
        {
            Debug.Log("Saved game");
            Instance._gameSaver.SaveAll(Instance.savableObjects);
        }

        public static void Load()
        {
            Instance._gameLoader.Load(Instance.prototypes);
        }

        public static bool SaveExists()
        {
            return Instance._gameLoader.SaveExists();
        }
    }
}