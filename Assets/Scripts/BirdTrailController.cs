using System.Collections;
using System.IO.IsolatedStorage;
using System.Threading;
using UnityEngine;

public class BirdTrailController: MonoBehaviour
{
    private TrailRenderer[] _renderers;
    private NoclipPlayerController _playerController;
    public float toggleSpeed = 0;
    public float currentTime = 0;

    void Awake()
    {
        _renderers = GetComponentsInChildren<TrailRenderer>();
        _playerController = GetComponent<NoclipPlayerController>();
    }

    void Update()
    {
    }

    public void Trail()
    {
        FadeInTrail();
    }

    public void SetTrailTime(double d)
    {
        foreach (var trailRenderer in _renderers)
        {
            trailRenderer.time = (float)d;
        } 
    }

    public void StopTrail()
    {
        FadeOutTrail();
    }
    
    public void FadeOutTrail()
    {
        StartCoroutine(nameof(DisappearTrail));
    }

    public IEnumerator  DisappearTrail()
    {
        float startTime = _renderers[0].time;
        for (int i = 0; i < 5; i++)
        {
            foreach (var trailRenderer in _renderers)
            {
                trailRenderer.time -= startTime/5;
            }
            yield return new WaitForSeconds(0.1f);
        }

        foreach (var trailRenderer in _renderers)
        {
                trailRenderer.emitting = false;
        }

    }

    public IEnumerator  AppearTrail()
    {
        foreach (var trailRenderer in _renderers)
        {
            trailRenderer.emitting = true;
        }
        float endTime = 2;
        for (int i = 0; i < 5; i++)
        {
            foreach (var trailRenderer in _renderers)
            {
                trailRenderer.time += endTime/5;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FadeInTrail()
    {
        StartCoroutine(AppearTrail());
    }

}