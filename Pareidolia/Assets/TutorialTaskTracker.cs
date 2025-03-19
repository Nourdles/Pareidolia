using UnityEngine;
using System;
/// <summary>
/// Keep track of what tasks have been completed and respond accordingly (event, advance to next level, etc)
/// 
/// </summary>
public class TutorialTaskTracker : MonoBehaviour
{
    private int numTasksCompleted = 0;
    private int numTasksGoal = 1;

    // Simple tasks
    public MakeBedTask makeBedTask;

    public DoorInteraction doorInteraction;

    // canvas for fading out level
    public FadeExitScene FadeOutCanvas;

    private void OnEnable()
    {
        MakeBedTask.CrossOutTaskEvent += CountSimpleTasksCompleted;
        doorInteraction.DoorFirstOpeningEvent += BridgeTaskCompleted;

    }

    private void OnDisable()
    {
        MakeBedTask.CrossOutTaskEvent -= CountSimpleTasksCompleted;
        //BedInteraction.BedInteractionEvent -= CountTasksCompleted;
        doorInteraction.DoorFirstOpeningEvent -= BridgeTaskCompleted;
    }

    private void CountSimpleTasksCompleted(int taskNum)
    {
        numTasksCompleted++;
        Debug.Log("A task has been completed");
        if (numTasksCompleted == numTasksGoal)
        {
            // let the player do the bridge task once all simple tasks are completed
            doorInteraction.UnlockDoor();
        }
    }

    private void BridgeTaskCompleted()
    {
        // start fading out the level, which will trigger a scene change
        FadeOutCanvas.FadeOutExit();
    }

}
