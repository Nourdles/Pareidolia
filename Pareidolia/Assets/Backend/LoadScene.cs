using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logic for the "play" button: load first scene of the game
/// </summary>

public class LoadScene : MonoBehaviour
{
    // load the tutorial level
    public static void LoadTutorialScene()
    {
        //FadeToLevel();
        SceneManager.LoadSceneAsync("TutorialLevel");
    }
    
    public static void LoadMorningScene()
    {
        //FadeToLevel();
        SceneManager.LoadSceneAsync("MorningLevel");
    }



}
