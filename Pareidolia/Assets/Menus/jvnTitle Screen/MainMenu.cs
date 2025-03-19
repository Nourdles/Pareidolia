using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeExitScene FadeOutCanvas;
    public void PlayGame()
    {

        Debug.Log("Play Game");
        // initialize game state;
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
