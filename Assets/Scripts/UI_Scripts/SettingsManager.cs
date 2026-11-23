using AudioScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class SettingsManager : MonoBehaviour
    {
        public Slider globalVolumeSlider;
        public Slider musicVolumeSlider;

        void Start()
        {
            globalVolumeSlider.value = AudioListener.volume;
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

            globalVolumeSlider.onValueChanged.AddListener(SetGlobalVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        private void SetMusicVolume(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            if (MusicController.Instance != null)
            {
                MusicController.Instance.SetVolume(value);
            }
        }

        private void SetGlobalVolume(float value)
        {
            PlayerPrefs.SetFloat("GlobalVolume", value);
            AudioListener.volume = value;
        }
    }
}