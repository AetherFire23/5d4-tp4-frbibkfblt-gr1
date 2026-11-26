using System;
using UnityEngine;

public class PaperNoteScript : MonoBehaviour
{
    [SerializeField] public string Text { get; set; }

    // Si tu rentres a proximite du player

    public void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<NoclipPlayerController>();
        if (player is null)
        {
            return;
        }
    }


    public void OnCollisionExit(Collision collision)
    {
        var player = collision.gameObject.GetComponent<NoclipPlayerController>();
        if (player is null)
        {
            return;
        }
        
        
        
    }

    // display Active le panel 
}