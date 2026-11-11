
using System.Collections.Generic;
using UnityEngine;

public class AllItemsManager : MonoBehaviour
{
    public static AllItemsManager instance;
    private List<GameObject> allItems = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void registerItem(GameObject go)
    {
        allItems.Add(go);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
