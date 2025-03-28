using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public PauseMenuManager pauseMenuManager;

    private bool isPaused = false;

    void Start()
    {
        // make sure pause menu is hidden at the beginning
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // ESC key or Start button (JoystickButton7)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // "B" can still resume if you're in main pause menu
        if (isPaused && Input.GetKeyDown(KeyCode.JoystickButton1) && pauseMenuCanvas.activeSelf)
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0.00001f;

        pauseMenuCanvas.SetActive(true);
        pauseMenuManager.ShowPauseMainMenu();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenuCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
