using Knight.Pure;
using SaveSystem;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    public KnightState Knight { get; set; } = new KnightState();

    private AudioClip _audioClip;

    private BirdInventory birdInventory;

    public void Awake()
    {
        Persistance.RegisterKnightRegion(this);
        SceneLoadedListener sceneLoadedLister = FindFirstObjectByType<SceneLoadedListener>();
        sceneLoadedLister.IsKnightSceneLoaded = true;
        sceneLoadedLister.LoadData();
        birdInventory = FindAnyObjectByType<BirdInventory>();
        Knight.TriggerOnKnightFullyCured += (state) => Debug.Log("Cured");
        Knight.TriggerOnKnightDeath += (state) => Debug.Log("Death");

        var statusPanel = FindFirstObjectByType<KnightStatusPanel>();

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

        Destroy(foodManager.gameObject);
    }
}