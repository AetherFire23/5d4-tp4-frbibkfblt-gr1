using NUnit.Framework;
using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    public GameObject[] spawnableItems;
    private GameObject SpawnedItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnedItem = spawnableItems[Random.Range(0, spawnableItems.Length)];
        Instantiate(SpawnedItem,this.gameObject.transform.position,this.gameObject.transform.rotation);
    }
}
