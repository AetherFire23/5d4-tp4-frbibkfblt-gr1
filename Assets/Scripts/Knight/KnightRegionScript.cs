using Knight.Pure;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    public KnightState Knight { get; set; } = new KnightState();
    private BirdInventory birdInventory;

    public void Awake()
    {
        birdInventory = FindAnyObjectByType<BirdInventory>();
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