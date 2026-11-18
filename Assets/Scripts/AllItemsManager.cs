using System.Collections.Generic;
using UnityEngine;

public class AllItemsManager : MonoBehaviour
{
    public static AllItemsManager instance;
    private List<GameObject> allItems = new List<GameObject>();
    
    private void Awake()
    {
        instance = this;
    }

    public void RegisterItem(GameObject go)
    {
        allItems.Add(go);
    }
}
