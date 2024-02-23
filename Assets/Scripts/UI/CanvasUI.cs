using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    [Header("Play/Pause Button")]
    [Space(5)]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseButton;

    private bool _isTogglingCanvas = false;

    public void OpenPauseCanvas()
    {
        if (!_isTogglingCanvas)
        {
            StartCoroutine(OpenPauseCanvasCoroutine());
        }
    }

    public void ClosePauseCanvas()
    {
        if (!_isTogglingCanvas)
        {
            StartCoroutine(ClosePauseCanvasCoroutine());
        }
    }

    private IEnumerator ClosePauseCanvasCoroutine()
    {
        _isTogglingCanvas = true;

        UIManager.Instance.ClosePauseCanvas();
        _pauseButton.gameObject.SetActive(true);
        _playButton.gameObject.SetActive(false);

        yield return null;

        _isTogglingCanvas = false;
    }

    private IEnumerator OpenPauseCanvasCoroutine()
    {
        _isTogglingCanvas = true;

        UIManager.Instance.OpenPauseCanvas();
        _pauseButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(true);

        yield return null;

        _isTogglingCanvas = false;
    }



    private void Awake()
    {
        _playButton.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
    }
}
