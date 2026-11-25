using System.Collections.Generic;
using Knight;
using Knight.Pure;
using Unity.VisualScripting;
using UnityEngine;

namespace SaveSystem
{
    public class Persistance
    {
        private GameSaver _gameSaver;
        private GameLoader _gameLoader;
        private List<ObjectData> foodItems = new List<ObjectData>();
        private ObjectData playerData;
        private FoodItemFactory foodItemFactory;
        private GameObject player;
        private KnightRegionScript knightRegionSript;

        public static Persistance Instance = new(
            new GameLoader(),
            new GameSaver()
        );

        private Persistance(GameLoader gameLoader, GameSaver gameSaver)
        {
            _gameLoader = gameLoader;
            _gameSaver = gameSaver;
        }

        public static void RegisterFoodItem(ObjectData foodItemData)
        {
            Instance.foodItems.Add(foodItemData);
        }

        public static void RegisterPlayerData(ObjectData playerData)
        {
            Instance.playerData = playerData;
        }

        public static void RegisterPlayer(GameObject gameObject)
        {
            Instance.player = gameObject;
            
        }

        // TODO: Crer la fonction REgister pour 

        public static void RegisterKnightRegion(KnightRegionScript knightState)
        {
            Instance.knightRegionSript = knightState;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void Save(string saveDir)
        {
            Instance._gameSaver.SavePlayer(Instance.playerData, saveDir);
            Instance._gameSaver.SaveFoodItems(Instance.foodItems, saveDir);

            Transform transform = Instance.knightRegionSript.transform;
            KnightRegionSave knightRegionSave =
                new KnightRegionSave(transform, "MyKnightSexyData", Instance.knightRegionSript.Knight);
            Instance._gameSaver.SaveKnightStatus(new List<ObjectData> { knightRegionSave }, saveDir);
        }
 
        public static void Load(string saveName)
        {
            List<ObjectData> foodItems = Instance._gameLoader.GetItemsFromSave(saveName);
            ObjectData newPlayerData = Instance._gameLoader.GetPlayerDataFromSave(saveName);
            KnightRegionSave knightRegionSave = Instance._gameLoader.GetKnightRegionSave(saveName);
            Instance.knightRegionSript.Knight = knightRegionSave.KnightState;
            Instance.foodItemFactory.CreateAllFromSave(foodItems);
            SetPlayerData(newPlayerData);
        }

        private static void SetPlayerData(ObjectData objectData)
        {
            Instance.player.transform.position = objectData.Position;
            Instance.player.transform.rotation = Quaternion.Euler(objectData.Rotation);
            Instance.player.transform.localScale = objectData.Scale;
        }

        public static void SetFoodItemFactory(FoodItemFactory foodItemFactory)
        {
            Instance.foodItemFactory = foodItemFactory;
        }

        public static bool SaveExists(string saveName)
        {
            return Instance._gameLoader.SaveExists(saveName);
        }

        public static void ClearFoodItems()
        {
            Instance.foodItems.Clear();
        }
    }
}