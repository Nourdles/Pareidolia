using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play Game");
        GameStateManager.StartTutorial();
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
