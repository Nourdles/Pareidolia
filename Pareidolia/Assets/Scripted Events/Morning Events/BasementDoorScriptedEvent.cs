using System;
using UnityEngine;
using FMODUnity;
/// <summary>
/// Script to open the basement door after the make breakfast task is completed, encouraging the player to go to the basement.
/// </summary>

public class BasementDoorScriptedEvent : MonoBehaviour
{
    private TaskManager taskManager;
    [SerializeField] DoorInteraction basementDoorInteraction;
    [SerializeField] DoorInteraction bedroomDoorInteraction;
    [SerializeField] GameObject basementDoorStain;
    private bool eventTriggered = false;
    public static event Action<string> BasementDoorDialogueEvent;
    public string doorOpenBasementSFX = "event:/SFX/DoorOpenBasement";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basementDoorStain.SetActive(false);
        taskManager = UnityEngine.Object.FindFirstObjectByType<TaskManager>();

        /*basementDoorInteraction = GetComponent<DoorInteraction>();

        if (basementDoorInteraction == null)
        {
            Debug.LogError("ScriptedDoorEvent: No DoorInteraction found on this object.");
        } */

        BowlInteraction.BreakfastMadeEvent += OnTaskCompleted;
        SilhouetteFlickerEvent.EventEnd += EndEvent;
    }

    private void OnDestroy()
    {
        //Task.CompleteTaskEvent -= OnTaskCompleted;
        SilhouetteFlickerEvent.EventEnd -= EndEvent;

    }

    // after completing two tasks, unlock the basement door.
    private void OnTaskCompleted()
    {
        if (!eventTriggered)
        {
            eventTriggered = true;

            // unlock basement door
            basementDoorInteraction.UnlockDoor();
            basementDoorInteraction.interact(null);
            BasementDoorDialogueEvent?.Invoke("What was that? The basement...?");

            // Play the FMOD sound here
            FMOD.Studio.EventInstance doorOpenEvent = FMODUnity.RuntimeManager.CreateInstance(doorOpenBasementSFX);
            doorOpenEvent.start();
            doorOpenEvent.release();

            // show stain beside basement door to lead player towards it
            basementDoorStain.SetActive(true);

            // 
            // lock bedroom door (so player has to go into basement)
            // bedroomDoorInteraction.LockDoor();
            // set the dialogue for the player attempting to enter the bedroom
            // bedroomDoorInteraction.SetLockedDialogue("...I should check out the basement first.");


        }
    }

    public void EndEvent()
    {
        // unlock bedroom door
        bedroomDoorInteraction.UnlockDoor();
    }

}
