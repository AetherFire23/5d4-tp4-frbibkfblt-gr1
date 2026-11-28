using UnityEngine;
using UnityEngine.Audio;

namespace AudioScripts
{
    public class MusicZone : MonoBehaviour
    {
        public AudioResource music;

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<NoclipPlayerController>() == null) return;
            if (MusicController.Instance == null) return;

            MusicController.Instance.Play(music);
        }
    }
}