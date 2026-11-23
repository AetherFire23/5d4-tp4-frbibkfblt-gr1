using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityPipeline.Microsoft.CodeAnalysis.Operations;

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

        // ReSharper disable Unity.PerformanceAnalysis
        public static void Save(string saveDir)
        {
            Instance._gameSaver.SavePlayer(Instance.playerData, saveDir);
            Instance._gameSaver.SaveFoodItems(Instance.foodItems, saveDir);
        }

        public static void Load(string saveName)
        {
            List<ObjectData> foodItems = Instance._gameLoader.GetItemsFromSave(saveName);
            ObjectData newPlayerData = Instance._gameLoader.GetPlayerDataFromSave(saveName);

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