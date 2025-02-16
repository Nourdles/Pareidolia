using UnityEngine;
using System;
/// <summary>
/// Keep track of what tasks have been completed and respond accordingly (event, advance to next level, etc)
/// 
/// </summary>
public class TutorialTaskTracker : MonoBehaviour
{
    private int numTasksCompleted = 0;
    private int numTasksGoal = 2;

    public MakeBedTask makeBedTask;
    public DoorInteraction doorInteraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (makeBedTask.isCompleted())
        {
            doorInteraction.UnlockDoor();
        }
    }
    private void OnEnable()
    {
        BedInteraction.BedInteractionEvent += CountTasksCompleted;
        doorInteraction.DoorFirstOpeningEvent += CountTasksCompleted;

    }

    private void OnDisable()
    {
        BedInteraction.BedInteractionEvent -= CountTasksCompleted;
        doorInteraction.DoorFirstOpeningEvent -= CountTasksCompleted;
    }

    private void CountTasksCompleted()
    {
        numTasksCompleted++;
        Debug.Log("A task has been completed");
        if (numTasksCompleted == numTasksGoal)
        {
            GameStateManager.StartMorning();
        }
    }
}
