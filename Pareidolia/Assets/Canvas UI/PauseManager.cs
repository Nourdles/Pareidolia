using System;
using UnityEngine;
using FMODUnity;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public PauseMenuManager pauseMenuManager;

    private bool isPaused = false;
    private FMOD.Studio.Bus gameBus;
    private FMOD.Studio.Bus uiBus;


    void Start()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;

        // Get a reference to the bus
        gameBus = RuntimeManager.GetBus("bus:/Game");
        uiBus = RuntimeManager.GetBus("bus:/UI");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.JoystickButton9))
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

        for (int i = 0; i <= 19; i++)
        {
            if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "JoystickButton" + i)))
            {
                Debug.Log("Pressed: JoystickButton" + i);
            }
        }

    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0.00001f;

        pauseMenuCanvas.SetActive(true);
        pauseMenuManager.ShowPauseMainMenu();

        // Pause only game sounds not master
        gameBus.setPaused(true);
        uiBus.setPaused(false);
        // prevent player from being able to open/close the notepad
        OpenCloseNote.isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Clear glows before hiding UI
        GlowHoverEffect[] glows = pauseMenuCanvas.GetComponentsInChildren<GlowHoverEffect>(false);
        foreach (var glow in glows)
        {
            if (glow.gameObject.activeInHierarchy)
            {
                glow.ForceGlowOff();
            }
        }

        pauseMenuCanvas.SetActive(false);
        // Unpause game sounds
        gameBus.setPaused(false);
        uiBus.setPaused(true);
        // enable opening/closing notepad again
        OpenCloseNote.isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}