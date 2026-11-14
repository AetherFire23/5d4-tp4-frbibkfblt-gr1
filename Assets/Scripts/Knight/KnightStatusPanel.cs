using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightStatusPanel : MonoBehaviour
{
    [SerializeField] private float factor;
    private BirdInventory bird;

    [SerializeField] private List<Image> images = new List<Image>();

    private void Awake()
    {
        bird = FindFirstObjectByType<BirdInventory>();
    }

    public void Update()
    {
        // Gerer la visibilite  plus on est loin
        //
        float dist = Vector3.Distance(this.transform.position, bird.transform.position);
        
        foreach (Image image in this.images)
        {
            float opacity = (factor * dist) + 1;
            Color color = image.color;
            color.a =opacity;
           image.color = color;
        
            Debug.Log(image.color);
        }
    }

    private void LateUpdate()
    {
        var dir = Camera.main.transform.position - this.transform.position;
        this.transform.rotation = Camera.main.transform.rotation;
    }
}