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

    public void initializer(FoodType foodType)
    {
        this.foodType = foodType;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void attachItem(Transform oiseau)
    {
        this.transform.SetParent(oiseau);
    }
    public void detachItem()
    {
        this.transform.SetParent(null);
    }
    public FoodType eat()
    {
        Destroy(this.gameObject);
        return this.foodType;
    }
}
public enum FoodType
{
    GoodFood,BadFood
}