using System;
using NUnit.Framework;
using SaveSystem;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemSpotManager: MonoBehaviour
{
    private class ItemSpotManagerGameData : ObjectData
    {
        public int itemGeneratedIndex;

        public ItemSpotManagerGameData(Transform transform, int itemGeneratedIndex, string id) : base(transform)
        {
            this.itemGeneratedIndex = itemGeneratedIndex;
            Id = id;
        }
    }

    public GameObject[] spawnableItems;
    private GameObject SpawnedItem;
    private int itemGeneratedIndex = -1;

    public void Create()
    {
        if (spawnableItems == null) return;
        InstantiateFoodFromIndex(Random.Range(0, spawnableItems.Length), transform.position, Quaternion.identity);
    }

    private void InstantiateFoodFromIndex(int index, Vector3 position, Quaternion rotation)
    {
        itemGeneratedIndex = index;
        SpawnedItem = spawnableItems[itemGeneratedIndex];
        Instantiate(SpawnedItem, position, rotation);
    }
    
}