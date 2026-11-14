using System.Linq;
using UnityEngine;

public class BirdInventory : MonoBehaviour
{
    private FoodManager grabbedItem;
    [SerializeField] private float itemPickupRadius;

    void Update()
    {
        bool mustGrab = Input.GetKeyDown(KeyCode.E) && grabbedItem is null;
        bool mustDropItem = Input.GetKeyDown(KeyCode.E) && grabbedItem is not null;
        if (mustGrab)
        {
            Collider? foodHitWithinRadius = Physics.OverlapSphere(this.transform.position, itemPickupRadius)
                .Where(x => x.GetComponent<FoodManager>() is not null)
                .OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position))
                .FirstOrDefault();

            if (foodHitWithinRadius is null)
            {
                return;
            }

            grabbedItem = foodHitWithinRadius.GetComponent<FoodManager>();
            grabbedItem.AttachItem(this.transform);
        }
        else if (mustDropItem)
        {
           // StopGrabbing();
        }
    }

    public void StopGrabbing()
    {
        if (grabbedItem is null)
        {
            return;
        }

        grabbedItem.DetachItem();
        grabbedItem = null;
    }
}