using UnityEngine;
using FMODUnity;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public PauseMenuManager pauseMenuManager;

    private bool isPaused = false;
    private FMOD.Studio.Bus masterBus;

    void Start()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isPaused && Input.GetKeyDown(KeyCode.JoystickButton1))
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

        masterBus.setPaused(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenuCanvas.SetActive(false);

        masterBus.setPaused(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}