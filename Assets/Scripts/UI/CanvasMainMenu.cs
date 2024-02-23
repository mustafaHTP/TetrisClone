using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMainMenu : MonoBehaviour
{
    [Header("Volume Menu Config")]
    [Space(5)]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Header("Menus Config")]
    [Space(5)]
    [SerializeField] private float _menuTransitionPositionOffset;
    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _optionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(Utilities.SceneName.Game);
    }

    public void OpenOptionsMenu()
    {
        _mainMenu.DOLocalMoveX(-_menuTransitionPositionOffset, 1f);
        _optionsMenu.DOLocalMoveX(0f, 1f);
    }

    public void CloseOptionsMenu()
    {
        _optionsMenu.DOLocalMoveX(_menuTransitionPositionOffset, 1f);
        _mainMenu.DOLocalMoveX(0f, 1f);
    }

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

    public void QuitGame()
    {
        Application.Quit();
    }

    private void InitializeOptionsMenu()
    {
        //Set position
        _optionsMenu.localPosition += Vector3.right * _menuTransitionPositionOffset;

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

    private void Awake()
    {
        InitializeOptionsMenu();
    }
}
