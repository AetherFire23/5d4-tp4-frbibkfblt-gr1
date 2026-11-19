using NUnit.Framework;
using SaveSystem;
using UnityEngine;



public class ItemSpotManager : MonoBehaviour, IUniqueObject, IPrototypeObjectFactory
{
    private class ItemSpotManagerGameData: ObjectData
    {
        public bool hasGenerated;

        public ItemSpotManagerGameData(Transform transform, bool hasGenerated) : base(transform)
        {
            this.hasGenerated = hasGenerated;
        }
    }
    public GameObject[] spawnableItems;
    private GameObject SpawnedItem;
    private bool hasGenerated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Persistance.RegisterUnique(this);
        hasGenerated = true;
    }

    public ObjectData GetObjectData()
    {
        return new ItemSpotManagerGameData(transform, hasGenerated);
    }

    public void BuildFromObjectData(ObjectData objectData)
    {
        hasGenerated = ((ItemSpotManagerGameData) objectData).hasGenerated;
    }

    public void InstanciateFromObjectData(ObjectData objectData)
    {
        SpawnedItem = spawnableItems[Random.Range(0, spawnableItems.Length)];
        Persistance.RegisterPrototype(SpawnedItem.GetComponent<FoodManager>());
        Instantiate(SpawnedItem,this.gameObject.transform.position,this.gameObject.transform.rotation);
    }
}