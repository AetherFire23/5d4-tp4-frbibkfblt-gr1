using Knight;
using Knight.Pure;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    private KnightState knight;

    public void Awake()
    {
        this.knight = new KnightState();
    }

    public void OnCollisionEnter(Collision collision)
    {
        bool isReagent = collision.gameObject.tag == "Reagent";
        
        // Quand tu hit un reagent, 
    }
}