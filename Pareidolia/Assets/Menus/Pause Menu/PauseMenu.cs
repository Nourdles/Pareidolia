using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public PauseMenuManager pauseMenuManager;
    public string mainMenuSceneName = "jvnTitleScene";
    public static PauseManager pauseManager;

    public void ResumeGame()
    {
        Debug.Log("Resume Game");

        if (pauseManager == null)
        {
            pauseManager = FindObjectOfType<PauseManager>();
        }
        pauseManager.ResumeGame();
    }

    public void Options()
    {
        Debug.Log("Options");
        pauseMenuManager.ShowPauseOptionsMenu();
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}