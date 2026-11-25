using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class FoodItemFactory: MonoBehaviour
    {
        [SerializeField] public GameObject[] foodPrefabs;
        public void CreateAllFromSave(List<ObjectData> instanceFoodItems)
        {
            
            foreach (ObjectData foodItem in instanceFoodItems)
            {
                GameObject found = FindPrefabWithId(foodItem.Id);
                if (found != null)
                    Instantiate(FindPrefabWithId(foodItem.Id), foodItem.Position, Quaternion.Euler(foodItem.Rotation));
            }
        }

        private GameObject FindPrefabWithId(string id)
        {
            foreach (GameObject foodItem in foodPrefabs)
            {
                if (foodItem.name == id) return foodItem;
            }

            return null;
        }
    }
}