using UnityEngine;
using UnityEngine.Audio;

namespace AudioScripts
{
    public class SceneMusic : MonoBehaviour
    {
        public AudioResource music;

        void Start()
        {
            if (MusicController.Instance != null)
            {
                MusicController.Instance.Play(music);
            }
        }

    }
}