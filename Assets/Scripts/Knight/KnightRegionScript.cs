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
        bool isGoodFood = collision.gameObject.tag == "Reagent";
        // if (isGoodFood)
        // {
        //     this.knight.ReceiveGoodFood();
        // }
        // else
        // {
        //     this.knight.ReceiveBadFood();
        // }
        //
        // if (this.knight.IsDead)
        // {
        //     // TODO: Gameover, show panel to go back 
        // }
    }
}