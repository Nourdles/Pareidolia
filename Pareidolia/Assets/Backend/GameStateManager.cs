using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static RandomFaceSpawner faceSpawner;
    public static event Action<Levels> LevelChangeEvent;

    public static Levels levelState;
    private static bool faceSpawnOn;

    private void Awake()
    {
        // determine which level has been loaded at the start of the scene
        // this allows us to test and play levels directly without having to play through previous
        // levels to trigger a level change event
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TutorialLevel")
        {
            levelState = Levels.Tutorial;
            LevelChangeEvent?.Invoke(levelState);
        }
        else if (scene.name == "MorningLevel")
        {
            levelState = Levels.Morning;
            RandomFaceSpawner.EnableFaceSpawning();
            LevelChangeEvent?.Invoke(levelState);
        }
    }


    public static void MoveToNextLevel()
    {
        switch (levelState)
        {
            case Levels.MainMenu:
                StartTutorial();
                break;

            case Levels.Tutorial:
                StartMorning();
                break;

        }
    }


    private static void StartTutorial()
    {
        Debug.Log("Starting Tutorial");
        levelState = Levels.Tutorial;

        // load the tutorial scene
        LoadScene.LoadTutorialScene();
        LevelChangeEvent?.Invoke(levelState);

    }

    private static void StartMorning()
    {
        Debug.Log("Advancing to Morning Level");
        levelState = Levels.Morning;

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

        switch (levelState)
        {
            case Levels.Morning:
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
