using UnityEngine;
using UnityEngine.Rendering;

public class FoodManager : MonoBehaviour
{
    public FoodType foodType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AllItemsManager.instance.registerItem(this.gameObject);
    }

    public void Initializer(FoodType foodType)
    {
        this.foodType = foodType;
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void AttachItem(Transform oiseau)
    {
        this.transform.SetParent(oiseau);
        this.transform.localPosition = Vector3.zero + new Vector3(0, 0,0.9f);
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void DetachItem()
    {
        this.transform.SetParent(null);
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        this.transform.Translate(new Vector3(0,1,0), Space.World);

    }

    public FoodType Eat()
    {
        Destroy(this.gameObject);
        return this.foodType;
    }
}

public enum FoodType
{
    GoodFood,
    BadFood
}