using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool gamePaused = false;
    public GameObject pauseMenu;
    private InputAction interactKey;

    void Start()
    {
        interactKey = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (interactKey.WasPressedThisFrame())
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Debug.Log("Game Paused");
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Resume()
    {
        Debug.Log("Game Resumed");
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void LoadMainMenu()
    {

    }

    public void QuitGame()
    {

    }
}
