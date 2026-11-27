using System;
using System.Collections.Generic;
using Knight.Pure;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class KnightStatusPanel : MonoBehaviour
{
    [SerializeField] private KnightRegionScript knightRegion;
    [SerializeField] private float completeFadeDistance;
    [SerializeField] private float fullyOnDistance; // 

    private BirdInventory bird;
    private float MAXIMUM_ALPHA = 1;
    private List<Image> images;

    [SerializeField] private Image curse;
    [SerializeField] private Image bleeding;
    [SerializeField] private Image poison;
    [SerializeField] private Image hunger;


    [SerializeField] private Color preventedColor;
    [SerializeField] private Color persistentColor;

    private KnightState knight;

    private void Awake()
    {
        bird = FindFirstObjectByType<BirdInventory>();

        images = new List<Image>()
        {
            curse,
            bleeding,
            poison,
            hunger
        };

    }

    public void SubscribeToAfflictionUpdates()
    {
        foreach (var knightAffliction in knightRegion.Knight.Afflictions)
        {
            knightAffliction.TriggerOnUpdated += TriggerOnSetAffliction;
        }
    }

    public void TriggerOnSetAffliction(KnightAffliction affliction)
    {
        Image afflictionImg = affliction.KnightAfflictionType switch
        {
            KnightAfflictionTypes.Curse => this.curse,
            KnightAfflictionTypes.Hunger => this.hunger,
            KnightAfflictionTypes.Poison => this.poison,
            KnightAfflictionTypes.Bleeding => this.bleeding,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (affliction.IsCured)
        {
            Color color = new Color(afflictionImg.color.r, afflictionImg.color.g, afflictionImg.color.b,
                afflictionImg.color.a);
            afflictionImg.color = color;
            afflictionImg.enabled = false;
        }
        else
        {
            afflictionImg.enabled = true;
        }

        if (affliction.IsPrevented)
        {
            Color color = new Color(
                this.preventedColor.r,
                this.preventedColor.g,
                this.preventedColor.b,
                afflictionImg.color.a);
            afflictionImg.color = color;
            Debug.Log("Prevenetd" + afflictionImg.color.ToString());
        }

        if (affliction.IsPersistent)
        {
            Color color = new Color(
                this.persistentColor.r,
                this.persistentColor.g,
                this.persistentColor.b,
                afflictionImg.color.a);
            afflictionImg.color = color;
        }

        if (!affliction.IsPrevented && !affliction.IsPersistent)
        {
            Color color = new Color(
                1,
                1,
                1,
                afflictionImg.color.a);
            afflictionImg.color = color;
        }
    }

    public void Update()
    {
        HandlePanelOpacity();
    }

    private void HandlePanelOpacity()
    {
        // Fonction definie par parties pour la visibilite du panneau. 
        float dist = Vector3.Distance(this.transform.position, bird.transform.position);

        foreach (Image image in images)
        {
            // COmmence toujours a 1
            float opacity = MAXIMUM_ALPHA;

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
        }
    }

    private void LateUpdate()
    {
        var dir = Camera.main.transform.position - this.transform.position;
        this.transform.rotation = Camera.main.transform.rotation;
    }
}