using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public PauseMenuManager pauseMenuManager;
    public string mainMenuSceneName = "jvnTitleScene";

    public void ResumeGame()
    {
        Debug.Log("Resume Game");

        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
