using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    public void Options()
    {
        Debug.Log("Options");
        // SceneManager.LoadScene("Options");
    }
    public void Credits()
    {
        Debug.Log("Credits");
        // SceneManager.LoadScene("Credits");
    }
}
