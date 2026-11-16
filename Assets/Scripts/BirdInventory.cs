using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdInventory : MonoBehaviour
{
    HashSet<GameObject> gameobjects = new HashSet<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("GoodFood") || collider.gameObject.CompareTag("GoodFood"))
        {
            this.gameobjects.Add(collider.gameObject);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("BadFood") || collider.gameObject.CompareTag("GoodFood"))
        {
            this.gameobjects.Remove(collider.gameObject);
        }
    }

    private GameObject holding;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.gameobjects.Count > 0)
            {
                GameObject go = this.gameobjects.ElementAt(0);
                this.holding = go;
                FoodManager foodManager = go.GetComponent<FoodManager>();
                foodManager.AttachItem(this.transform);
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            print("drop");
            DropItem();
        }
    }

    public void DropItem()
    {
        FoodManager foodManager = holding.GetComponent<FoodManager>();
        foodManager.DetachItem();
    }
}