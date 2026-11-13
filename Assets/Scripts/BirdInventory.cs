using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BirdInventory : MonoBehaviour
{
    private FoodManager grabbedItem;
    [SerializeField] private float itemPickupRadius;

    void Update()
    {
        bool canGrab = Input.GetKeyDown(KeyCode.E) && grabbedItem is null;
        bool mustDropItem = Input.GetKeyDown(KeyCode.E) && grabbedItem is not null; 
        if (canGrab)
        {
            Collider? foodHitWithinRadius = Physics.OverlapSphere(this.transform.position, itemPickupRadius)
                .Where(x => x.GetComponent<FoodManager>() is not null)
                .OrderBy(x=> Vector3.Distance(x.transform.position, this.transform.position))
                .FirstOrDefault();

            if (foodHitWithinRadius is null)
            {
                Debug.Log("No item found");
                return;
            }

            grabbedItem = foodHitWithinRadius.GetComponent<FoodManager>();
            grabbedItem.AttachItem(this.transform);
        }
        else if (mustDropItem)
        {
            grabbedItem.DetachItem();
            grabbedItem = null;
        }
    }
}