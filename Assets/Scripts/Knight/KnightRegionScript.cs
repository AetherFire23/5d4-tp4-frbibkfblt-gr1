using Knight.Pure;
using SaveSystem;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _source;
    [SerializeField] private ParticleSystem fireParticles;

    [SerializeField] private Animator knightAnimator;

    private BirdInventory birdInventory;
    public KnightState Knight { get; set; } = new KnightState();


    public void Awake()
    {
        Persistance.RegisterKnightRegion(this);
        SceneLoadedListener sceneLoadedLister = FindFirstObjectByType<SceneLoadedListener>();
        sceneLoadedLister.IsKnightSceneLoaded = true;
        sceneLoadedLister.LoadData();
        birdInventory = FindAnyObjectByType<BirdInventory>();

        Knight.TriggerOnKnightFullyCured += GameManager.Instance.EndGame;
        Knight.TriggerOnKnightDeath += GameManager.Instance.EndGame;

        KnightStatusPanel statusPanel = FindFirstObjectByType<KnightStatusPanel>();
        statusPanel.SubscribeToAfflictionUpdates();
        this.Knight.Refresh();
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

    public void OnEndAnimation()
    {
        knightAnimator.SetBool("isTakingPill", false);
    }
}