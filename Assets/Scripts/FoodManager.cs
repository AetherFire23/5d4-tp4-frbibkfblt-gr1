using System;
using System.Collections.Generic;
using Knight.Pure;
using SaveSystem;
using UnityEngine;

public class FoodManager : MonoBehaviour, ISavableGameObject
{
    private Rigidbody rb;
    private Vector3 FORWARD_GRAB = new Vector3(0.0f, 0, 0.0f);
    private Vector3 DROP_HIGHER = new Vector3(0, 1, 0);

    [SerializeField] private string name;
    [SerializeField] private List<KnightAfflictionTypes> cures;
    [SerializeField] private List<KnightAfflictionTypes> inflicts;
    [SerializeField] private List<KnightAfflictionTypes> prevents;
    [SerializeField] private List<KnightAfflictionTypes> persists;

    public Reagent Reagent;
    public float sizeMultiplier = 1.2f;

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

    public ObjectData GetObjectData()
    {
        return new ObjectData(
            transform.position,
            transform.rotation.eulerAngles,
            transform.localScale,
            name
        );
    }

    public void BuildFromObjectData(ObjectData objectData)
    {
        this.transform.position = objectData.Position;
        this.transform.rotation = new Quaternion(objectData.Rotation.x, objectData.Rotation.y, objectData.Rotation.z, 0);
        this.transform.localScale = objectData.Scale;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

public enum FoodType
{
    GoodFood,
    BadFood
}