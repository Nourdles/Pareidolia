using UnityEngine;

public static class GameStateManager
{
    public static RandomFaceSpawner faceSpawner;


    public static Levels levelState;
    private static bool faceSpawnOn;
    


    public static void PauseGame()
    {
        // show pause screen
    }

    public static void StartTutorial()
    {
        levelState = Levels.Tutorial;
        // turn off face spawn just in case
        RandomFaceSpawner.DisableFaceSpawning();

    }

    public static void StartMorning()
    {
        Debug.Log("Advancing to Morning Level");
        levelState = Levels.Morning;
        // start face spawning
        RandomFaceSpawner.EnableFaceSpawning();
    }

    /*
    public static void StartAfternoon()
    {
        levelState = Levels.Afternoon;
        faceSpawnOn = true;
    }
    */

    /*
    public static void StartEvening()
    {
        levelState = Levels.Evening;
        faceSpawnOn = true;
    } 
    */


    /*
    public static void EndGame()
    {
        // do something
    }
    */


}
