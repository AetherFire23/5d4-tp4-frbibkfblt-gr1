using System;
using System.Collections.Generic;
using Knight.Pure;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 FORWARD_GRAB = new Vector3(0.0f, 0.0f, 0.9f);
    private Vector3 DROP_HIGHER = new Vector3(0.0f, 1.0f, 0.0f);

    [SerializeField] private string name;
    [SerializeField] private List<KnightAfflictionTypes> cures;
    [SerializeField] private List<KnightAfflictionTypes> inflicts;
    [SerializeField] private List<KnightAfflictionTypes> prevents;
    [SerializeField] private List<KnightAfflictionTypes> persists;
    public float sizeMultiplier = 1.2f;
    public Reagent Reagent;
    private void Awake()
    {
        this.Reagent = new Reagent(this.name, cures, inflicts, prevents, persists);
    }

    void Start()
    {
        AllItemsManager.instance.RegisterItem(gameObject);
        rb = GetComponent<Rigidbody>();
    }

    public void AttachItem(Transform oiseau)
    {
        transform.SetParent(oiseau);
        transform.rotation = Quaternion.identity;
        transform.localPosition = FORWARD_GRAB;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = false;
        transform.localScale /= sizeMultiplier;
    }

    public void DetachItem()
    {
        transform.SetParent(null);
        rb.constraints = RigidbodyConstraints.None;
        transform.Translate(DROP_HIGHER, Space.World);
        
        GetComponent<Collider>().enabled = true;
        transform.localScale *= sizeMultiplier;
    }
}

public enum FoodType
{
    GoodFood,
    BadFood
}