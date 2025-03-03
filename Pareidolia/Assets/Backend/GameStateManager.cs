using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static RandomFaceSpawner faceSpawner;
    public static event Action<Levels> LevelChangeEvent;

    public static Levels levelState;
    private static bool faceSpawnOn;
    

    public static void StartTutorial()
    {
        Debug.Log("Starting Tutorial");
        levelState = Levels.Tutorial;

        // load the tutorial scene
        LoadScene.LoadTutorialScene();
        LevelChangeEvent?.Invoke(levelState);

    }

    public static void StartMorning()
    {
        Debug.Log("Advancing to Morning Level");
        levelState = Levels.Morning;

        // fade out level

        // load the morning scene
        LoadScene.LoadMorningScene();
        // start face spawning
        RandomFaceSpawner.EnableFaceSpawning();

        LevelChangeEvent?.Invoke(levelState);
    }


    public static void Respawn()
    {
        // determine which level the player died in, then respawn at the start of the level
        // reload scene at beginning (restart all tasks, restore sanity)

        switch ((int)levelState)
        {
            case 1:
                LoadScene.LoadMorningScene();
                break;
        }
    }


    /*
    public static void StartAfternoon()
    {
        levelState = Levels.Afternoon;
        faceSpawnOn = true;
        LevelChangeEvent?.Invoke(levelState);
    }
    */

    /*
    public static void StartEvening()
    {
        levelState = Levels.Evening;
        faceSpawnOn = true;
        LevelChangeEvent?.Invoke(levelState);
    } 
    */


    /*
    public static void EndGame()
    {
        // do something
    }
    */


}
