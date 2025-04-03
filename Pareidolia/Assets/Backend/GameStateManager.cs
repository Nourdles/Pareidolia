using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static RandomFaceSpawner faceSpawner;
    public static event Action<Levels> LevelChangeEvent;

    public Image BlackPanel;

    public static Levels levelState;
    private static bool faceSpawnOn;

    private void Awake()
    {
        // determine which level has been loaded at the start of the scene
        // this allows us to test and play levels directly without having to play through previous
        // levels to trigger a level change event
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "IntroSequence")
        {
            levelState = Levels.IntroSequence;
            LevelChangeEvent?.Invoke(levelState);

        }
        else if (scene.name == "MorningCutscene")
        {  
            levelState = Levels.MorningCutscene;
            LevelChangeEvent?.Invoke(levelState);
        } 
        else if (scene.name == "TutorialLevel")
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
        else if (scene.name == "EndingCutscene")
        {
            levelState = Levels.EndingCutscene;
            LevelChangeEvent?.Invoke(levelState);
        }
        else if (scene.name == "Credits")
        {
            levelState = Levels.Credits;
            LevelChangeEvent?.Invoke(levelState);
        }
    }


    public static void MoveToNextLevel()
    {
        switch (levelState)
        {
            case Levels.MainMenu:
                StartIntroSequence();
                break;
            case Levels.IntroSequence:
                StartMorningCutscene();
                break;
            case Levels.MorningCutscene:
                StartTutorial();
                break;

            case Levels.Tutorial:
                StartMorning();
                break;
            case Levels.Morning:
                StartEndingCutscene();
                break;
            case Levels.EndingCutscene:
                StartCredits();
                break;
            default:
                StartTutorial();
                break;

        }
    }


    private static void StartIntroSequence()
    {
        Debug.Log("Starting Intro Sequence");
        levelState = Levels.IntroSequence;

        // load the intro sequence scene
        LoadScene.LoadIntroSequence();
        LevelChangeEvent?.Invoke(levelState);
    }

    private static void StartMorningCutscene()
    {
        Debug.Log("Starting Morning Cutscene");
        levelState = Levels.MorningCutscene;

        AudioManager.instance.StopAmbience();

        // load the morning cutscene scene
        LoadScene.LoadMorningCutscene();
        LevelChangeEvent?.Invoke(levelState);
    }
    
    private static void StartTutorial()
    {
        Debug.Log("Starting Tutorial");
        levelState = Levels.Tutorial;

        AudioManager.instance.StartRoomAmbience();

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

    private static void StartEndingCutscene()
    {
        Debug.Log("Starting Ending Cutscene");
        levelState = Levels.EndingCutscene;

        AudioManager.instance.StopAmbience();

        LoadScene.LoadEndingCutscene();
        LevelChangeEvent?.Invoke(levelState);
    }

    private static void StartCredits()
    {
        Debug.Log("Starting Credits");
        levelState = Levels.Credits;

        LoadScene.LoadCredits();
        LevelChangeEvent?.Invoke(levelState);
    }

    private static void EndGame()
    {
        Debug.Log("Game End");

        AudioManager.instance.StopAmbience();

        // play credits
        LoadScene.LoadGameEnd();

    }





    public Levels GetLevelState()
    {
        return levelState;
    }

    public static void Respawn()
    {
        // determine which level the player died in, then respawn at the start of the level
        // reload scene at beginning (restart all tasks, restore sanity)

        //Step 1: fade to black
       

        //Step 2: Add permanent stain at kill spot

        //Step 3: move player to bed
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
