using System;
using System.Collections.Generic;
using Knight.Pure;
using SaveSystem;
using UnityEngine;
using Random = System.Random;

public class FoodManager : MonoBehaviour
{
    private Rigidbody rb;
    
    private Vector3 FORWARD_GRAB = new Vector3(0,0 , -0.2f);
    private Vector3 DROP_HIGHER = new Vector3(0, 1, 0);

    [SerializeField] private string name;
    [SerializeField] private List<KnightAfflictionTypes> cures;
    [SerializeField] private List<KnightAfflictionTypes> inflicts;
    [SerializeField] private List<KnightAfflictionTypes> prevents;
    [SerializeField] private List<KnightAfflictionTypes> persists;
    [SerializeField] public List<AudioClip> clips;

    public Reagent Reagent;
    public float sizeMultiplier = 1.2f;
    [NonSerialized] public string Id;
    private AudioSource _soundPlayer;
    private bool audioPlayed = false;
    private bool isAttached = false;
    private bool isJustSpawned = true;

    private void Awake()
    {
        this.Reagent = new Reagent(this.name, cures, inflicts, prevents, persists);
        Id = gameObject.name;
    }

    void Start()
    {
        AllItemsManager.instance.RegisterItem(gameObject);
        rb = GetComponent<Rigidbody>();
        _soundPlayer = GetComponent<AudioSource>();
        Invoke(nameof(NotJustSpawned), 3f);
    }

    void NotJustSpawned()
    {
        isJustSpawned = false;
    }

    public void AttachItem(Transform oiseau)
    {
        transform.SetParent(oiseau);
        transform.rotation = Quaternion.identity;
        transform.localPosition = FORWARD_GRAB;
        Debug.Log("LOCAL POSITION OF FUCKING IETM" + transform.localPosition);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = false;
        transform.localScale /= sizeMultiplier;
        audioPlayed =  false;
        isAttached = true;
    }

    public void DetachItem()
    {
        transform.SetParent(null);
        rb.constraints = RigidbodyConstraints.None;
        transform.Translate(DROP_HIGHER, Space.World);
        GetComponent<Collider>().enabled = true;
        transform.localScale *= sizeMultiplier;
        isAttached = false;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !audioPlayed && !isAttached && !isJustSpawned)
        {
            int rndClip = new Random().Next(0, clips.Count);
            _soundPlayer.clip = clips[rndClip];
            _soundPlayer.pitch = (float)new Random().NextDouble() * 2;
            _soundPlayer.volume = collision.relativeVelocity.magnitude / 30;
            _soundPlayer.Play();
            audioPlayed = true;
        }
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