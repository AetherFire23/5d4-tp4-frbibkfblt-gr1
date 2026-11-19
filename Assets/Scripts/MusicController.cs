using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    AudioSource source;

    public float Volume => source.volume;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetVolume(float value)
    {
        source.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
