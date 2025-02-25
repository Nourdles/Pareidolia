using System;
using UnityEngine;

public static class GameStateManager
{
    public static RandomFaceSpawner faceSpawner;
    public static event Action<Levels> LevelChangeEvent;

    public static Levels levelState;
    private static bool faceSpawnOn;
    

    public static void StartTutorial()
    {
        levelState = Levels.Tutorial;
        // turn off face spawn just in case
        RandomFaceSpawner.DisableFaceSpawning();
        LevelChangeEvent?.Invoke(levelState);

    }

    public static void StartMorning()
    {
        Debug.Log("Advancing to Morning Level");
        levelState = Levels.Morning;
        // start face spawning
        RandomFaceSpawner.EnableFaceSpawning();
        LevelChangeEvent?.Invoke(levelState);
    }


    public static void Respawn()
    {
        // determine which level the player died in, then respawn at the start of that level
        // reload scene at beginning (restart all tasks, restore sanity)
        /*
        switch ((int)levelState)
        {
            case 1:

                break;
            case 2:
        }*/
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
