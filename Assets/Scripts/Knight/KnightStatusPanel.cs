using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightStatusPanel : MonoBehaviour
{
    [SerializeField] private float completeFadeDistance;
    [SerializeField] private float fullyOnDistance; // 
    
    private BirdInventory bird;
    private float MAXIMUM_ALPHA = 1;
    [SerializeField] private List<Image> images = new List<Image>();
    

    private void Awake()
    {
        bird = FindFirstObjectByType<BirdInventory>();
    }

    public void Update()
    {
        HandlePanelOpacity();
    }
    

    private void HandlePanelOpacity()
    {
        // Fonction definie par parties pour la visibilite du panneau. 
        float dist = Vector3.Distance(this.transform.position, bird.transform.position);

        foreach (Image image in this.images)
        {
            // COmmence toujours a 1
            float opacity = 1f;

            if (dist > fullyOnDistance)
            {
                // y =ax +b
                // Sauf que l'ampleur de la pente depend de ou on veut que ca commence et ou on veut que ca termine.  
                Vector2 p1 = new Vector2(fullyOnDistance, MAXIMUM_ALPHA);
                Vector2 p2 = new Vector2(completeFadeDistance, 0);
                float slope = (p1.y - p2.y) / (p1.x - p2.x);

                // Le parametre b aurait ete toujours = 1 si ca avait ete lineaire, suaf que la ca peut changer. 
                float bParametre = MAXIMUM_ALPHA - (slope * p1.x);
                opacity = (slope * dist) + bParametre;
            }

            Color color = image.color;
            color.a = opacity;
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