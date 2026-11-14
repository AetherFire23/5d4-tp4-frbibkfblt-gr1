using Knight.Pure;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    private KnightState knight;
    private BirdInventory birdInventory;

    public void Awake()
    {
        birdInventory = FindAnyObjectByType<BirdInventory>();
        knight = new KnightState();
    }

    public void OnCollisionEnter(Collision collision)
    {
        FoodManager? foodManager = collision.gameObject.GetComponent<FoodManager>();
        if (foodManager is null)
        {
            return;
        }
        
        this.birdInventory.StopGrabbing();

        knight.ReceiveReagent(foodManager.Reagent);

        Destroy(foodManager.gameObject);
        
    }
}