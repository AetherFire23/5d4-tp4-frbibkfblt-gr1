using System;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 FORWARD_GRAB = new Vector3(0, 0, 0.9f);
    private Vector3 DROP_HIGHER = new Vector3(0, 1, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AllItemsManager.instance.RegisterItem(gameObject);
        rb = GetComponent<Rigidbody>();
    }

    public void AttachItem(Transform oiseau)
    {
        transform.SetParent(oiseau);
        transform.localPosition = FORWARD_GRAB;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void DetachItem()
    {
        transform.SetParent(null);
        rb.constraints = RigidbodyConstraints.None;
        transform.Translate(DROP_HIGHER, Space.World);
    }
}

public enum FoodType
{
    GoodFood,
    BadFood
}