using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MainMenu : MonoBehaviour
{
    public FadeExitScene FadeOutCanvas;

    private static FMOD.Studio.EventInstance titleScreenMusicInstance;
    private FMOD.Studio.Bus uiBus;

    void Start()
    {
        if (!titleScreenMusicInstance.isValid())
        {
            titleScreenMusicInstance = RuntimeManager.CreateInstance("event:/Music/Title Screen Music");
            titleScreenMusicInstance.start();
        }

        uiBus = RuntimeManager.GetBus("bus:/UI");
    }

    public void PlayGame()
    {
        Debug.Log("Play Game");

        // Stop and release the title screen music
        if (titleScreenMusicInstance.isValid())
        {
            titleScreenMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            titleScreenMusicInstance.release();
            titleScreenMusicInstance.clearHandle();
        }

        uiBus.setPaused(true);

        GameStateManager.levelState = Levels.MainMenu;
        FadeOutCanvas.FadeOutExit();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void Credits()
    {
        Debug.Log("Credits");
    }
}
