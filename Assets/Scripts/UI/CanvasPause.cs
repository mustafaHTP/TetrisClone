using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasPause : MonoBehaviour
{
    public void Continue()
    {
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        GetComponent<Transform>().DOScale(0f, 1f).SetUpdate(true);
        GetComponent<CanvasGroup>().DOFade(0f, 1f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        });
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Utilities.SceneName.MainMenu);
    }
}
