using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    [Header("Play/Pause Button")]
    [Space(5)]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseButton;

    public void OpenPauseCanvas()
    {
        UIManager.Instance.OpenPauseCanvas();
        _pauseButton.gameObject.SetActive(false);
        _pauseButton.interactable = false;
        _playButton.gameObject.SetActive(true);
    }

    public void ClosePauseCanvas()
    {
        UIManager.Instance.ClosePauseCanvas();
        _pauseButton.gameObject.SetActive(true);
        _playButton.gameObject.SetActive(false);
        _playButton.interactable = false;
    }

    private void MakePlayButtonInteractable()
    {
        _playButton.interactable = true;
    }

    private void MakePauseButtonInteractable()
    {
        _pauseButton.interactable = true;
    }

    private void Awake()
    {
        _playButton.gameObject.SetActive(false);
        _playButton.interactable = false;
        _pauseButton.gameObject.SetActive(true);
        _pauseButton.interactable = true;
    }

    private void OnEnable()
    {
        UIManager.OnOpenPauseCanvas += MakePlayButtonInteractable;
        UIManager.OnClosePauseCanvas += MakePauseButtonInteractable;
    }

    private void OnDisable()
    {
        UIManager.OnOpenPauseCanvas -= MakePlayButtonInteractable;   
        UIManager.OnClosePauseCanvas -= MakePauseButtonInteractable;
    }
}
