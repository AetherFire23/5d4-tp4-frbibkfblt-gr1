using Knight;
using UnityEngine;

public class KnightRegionScript : MonoBehaviour
{
    private KnightStats knight;

    public void Awake()
    {
        this.knight = new KnightStats();
    }

    public void OnCollisionEnter(Collision collision)
    {
        bool isGoodFood = collision.gameObject.tag == "GoodFood";
        if (isGoodFood)
        {
            this.knight.ReceiveGoodFood();
        }
        else
        {
            this.knight.ReceiveBadFood();
        }

        if (this.knight.IsDead)
        {
            // TODO: Gameover, show panel to go back 
        }
    }
}