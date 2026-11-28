using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicController : MonoBehaviour
    {
        public static MusicController Instance;

        public float fadeTime = 1.5f;

        public AudioSource current;
        public AudioSource other;
        public float musicVolume;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1f);

            current = GetComponent<AudioSource>();
            current.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);

            other = gameObject.AddComponent<AudioSource>();
            other.playOnAwake = false;
            other.loop = current.loop;
            other.outputAudioMixerGroup = current.outputAudioMixerGroup;
            other.spatialBlend = current.spatialBlend;
            other.priority = current.priority;
        }

        public void SetVolume(float value)
        {
            musicVolume = value;
            current.volume = value;
        }

        public void Play(AudioResource music)
        {
            if (current.resource == music) return;

            (current, other) = (other, current); //juste pour toi fred


            if (current.resource != music || !current.isPlaying)
            {
                current.resource = music;
                current.volume = 0f;
                if (music != null) current.Play();
            }

            StopAllCoroutines();
            StartCoroutine(Crossfade());
        }

        IEnumerator Crossfade()
        {
            float inStart = current.volume;
            float outStart = other.volume;

            for (float time = 0f; time < fadeTime; time += Time.unscaledDeltaTime)
            {
                float fadeCoefficient = time / fadeTime;
                current.volume = Mathf.Lerp(inStart, musicVolume, fadeCoefficient);
                other.volume = Mathf.Lerp(outStart, 0f, fadeCoefficient);
                yield return null;
            }

            current.volume = musicVolume;
            other.volume = 0f;
            other.Stop();
        }
    }
}