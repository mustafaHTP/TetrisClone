using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(Utilities.SceneName.Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
