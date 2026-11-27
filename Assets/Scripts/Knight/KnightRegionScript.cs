using System;
using Endgame;
using Knight.Pure;
using SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

public class KnightRegionScript : MonoBehaviour
{
    public KnightState Knight { get; set; } = new KnightState();

    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _source;
    [SerializeField] private ParticleSystem fireParticles;

    private BirdInventory birdInventory;

    [SerializeField] private Animator knightAnimator;


    public void Awake()
    {
        Persistance.RegisterKnightRegion(this);
        SceneLoadedListener sceneLoadedLister = FindFirstObjectByType<SceneLoadedListener>();
        sceneLoadedLister.IsKnightSceneLoaded = true;
        sceneLoadedLister.LoadData();
        birdInventory = FindAnyObjectByType<BirdInventory>();

        EndGameDontDestroyOnLoad endgameScript = FindFirstObjectByType<EndGameDontDestroyOnLoad>();
        
        Knight.TriggerOnKnightFullyCured += endgameScript.DoWinningEndgame;
        Knight.TriggerOnKnightDeath += endgameScript.DoLosingEndgame;

        KnightStatusPanel statusPanel = FindFirstObjectByType<KnightStatusPanel>();
        statusPanel.SubscribeToAfflictionUpdates();
        this.Knight.Refresh();
    }
    
    public void OnEndAnimation()
    {
        knightAnimator.SetBool("isTakingPill", false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        FoodManager? foodManager = collision.gameObject.GetComponent<FoodManager>();
        if (foodManager is null)
        {
            return;
        }

        this.birdInventory.StopGrabbing();

        Knight.ReceiveReagent(foodManager.Reagent);
        knightAnimator.SetBool("isTakingPill", true);
        Invoke("OnEndAnimation", 2);
        fireParticles.Play();
        _source.PlayOneShot(_audioClip);
        Destroy(foodManager.gameObject);
    }
}