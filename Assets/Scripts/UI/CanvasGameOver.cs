using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScoreTMP;

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(Utilities.SceneName.MainMenu);
    }

    private void OnEnable()
    {
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("Score manager not found...");
            return;
        }

        _highScoreTMP.text = scoreManager.GetHighScore().ToString();
    }
}
