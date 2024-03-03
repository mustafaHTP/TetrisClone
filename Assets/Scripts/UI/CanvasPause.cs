using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasPause : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    public void OnChangeMusicVolume()
    {
        //Audio mixer volume has logarithmic scale
        float newVolume = Mathf.Log10(_musicVolumeSlider.value) * 20f;
        _audioMixer.SetFloat(Utilities.AudioMixerParams.MusicVolume, newVolume);
    }

    public void OnChangeSFXVolume()
    {
        //Audio mixer volume has logarithmic scale
        float newVolume = Mathf.Log10(_sfxVolumeSlider.value) * 20f;
        _audioMixer.SetFloat(Utilities.AudioMixerParams.SFXVolume, newVolume);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Utilities.SceneName.MainMenu);
    }

    private void InitializeOptionsMenu()
    {
        //Set slider values
        //Music
        _audioMixer.GetFloat(Utilities.AudioMixerParams.MusicVolume, out float currentVolume);
        currentVolume = Mathf.Pow(10f, currentVolume / 20f);
        _musicVolumeSlider.value = currentVolume;

        //SFX
        _audioMixer.GetFloat(Utilities.AudioMixerParams.SFXVolume, out currentVolume);
        currentVolume = Mathf.Pow(10f, currentVolume / 20f);
        _sfxVolumeSlider.value = currentVolume;
    }

    private void OnEnable()
    {
        InitializeOptionsMenu();
    }
}
