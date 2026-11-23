using System;
using System.Collections.Generic;
using Knight.Pure;
using SaveSystem;
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

    public Reagent Reagent;
    public float sizeMultiplier = 1.2f;
    [NonSerialized] public string Id; 

    private void Awake()
    {
        this.Reagent = new Reagent(this.name, cures, inflicts, prevents, persists);
        Id = gameObject.name;
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

    public ObjectData GetObjectData()
    {
        return new FoodObjectData(
            transform,
            cures,
            inflicts,
            prevents,
            persists,
            name,
            Id
        );
    }

    public class FoodObjectData: ObjectData
    {
        public string name;
        public List<KnightAfflictionTypes> cures;
        public List<KnightAfflictionTypes> inflicts;
        public List<KnightAfflictionTypes> prevents;
        public List<KnightAfflictionTypes> persists;

        public FoodObjectData(Transform transform, 
            List<KnightAfflictionTypes> cures,
            List<KnightAfflictionTypes> inflicts,
            List<KnightAfflictionTypes> prevents,
            List<KnightAfflictionTypes> persists,
            string name,
            string id
            ) : base(transform)
        {
            this.cures = cures;
            this.inflicts = inflicts;
            this.prevents = prevents;
            this.persists = persists;
            this.name = name;
            Id = id;
        }
    }
}

public enum FoodType
{
    GoodFood,
    BadFood
}