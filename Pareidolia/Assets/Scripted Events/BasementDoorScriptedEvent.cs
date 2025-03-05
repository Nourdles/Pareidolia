using UnityEngine;
/// <summary>
/// Script to open the basement door after the make breakfast task is completed, encouraging the player to go to the basement.
/// </summary>

public class BasementDoorScriptedEvent : MonoBehaviour
{
    private TaskManager taskManager;
    private DoorInteraction doorInteraction;
    private bool eventTriggered = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        taskManager = Object.FindFirstObjectByType<TaskManager>();

        doorInteraction = GetComponent<DoorInteraction>();

        if (doorInteraction == null)
        {
            Debug.LogError("ScriptedDoorEvent: No DoorInteraction found on this object.");
        }

        Task.CompleteTaskEvent += OnTaskCompleted;
    }

    private void OnDestroy()
    {
        Task.CompleteTaskEvent -= OnTaskCompleted;
    }

    private void OnTaskCompleted()
    {
        if (!eventTriggered && taskManager.IsMorningComplete())
        {
            eventTriggered = true;
            doorInteraction.UnlockDoor();
            doorInteraction.interact(null);
        }
    }
}
