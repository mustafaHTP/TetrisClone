using DG.Tweening;
using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static event Action OnOpenPauseCanvas;
    public static event Action OnClosePauseCanvas;

    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private Canvas _pauseCanvas;

    public void OpenGameOverCanvas()
    {
        _gameOverCanvas.GetComponent<Transform>().localScale = Vector2.zero;
        _gameOverCanvas.GetComponent<CanvasGroup>().alpha = 0f;

        _gameOverCanvas.gameObject.SetActive(true);

        _gameOverCanvas.GetComponent<Transform>().DOScale(1f, 1f);
        _gameOverCanvas.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
        {
            _gameOverCanvas.GetComponent<CanvasGroup>().interactable = true;
            _gameOverCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
    }

    public void OpenPauseCanvas()
    {
        //Open only if canvas is disabled
        if (_pauseCanvas.gameObject.activeSelf) return;

        Time.timeScale = 0f;

        _pauseCanvas.GetComponent<CanvasGroup>().alpha = 0f;

        _pauseCanvas.gameObject.SetActive(true);

        //SetUpdate() is used because game is stopped
        _pauseCanvas.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetUpdate(true).OnComplete(() =>
        {
            _pauseCanvas.GetComponent<CanvasGroup>().interactable = true;
            _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            OnOpenPauseCanvas?.Invoke();
        });
    }

    public void ClosePauseCanvas()
    {
        //Open only if canvas is enabled
        if (!_pauseCanvas.gameObject.activeSelf) return;

        _pauseCanvas.GetComponent<CanvasGroup>().interactable = false;
        _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

        _pauseCanvas.GetComponent<CanvasGroup>().DOFade(0f, 1f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            _pauseCanvas.gameObject.SetActive(false);
            OnClosePauseCanvas?.Invoke();
        });
    }

    private void DisableGameOverCanvas()
    {
        _gameOverCanvas.GetComponent<CanvasGroup>().interactable = false;
        _gameOverCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _gameOverCanvas.gameObject.SetActive(false);
    }

    private void DisablePauseCanvas()
    {
        _pauseCanvas.GetComponent<CanvasGroup>().interactable = false;
        _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _pauseCanvas.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;

        DisableGameOverCanvas();
        DisablePauseCanvas();
    }
}
