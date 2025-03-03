using UnityEngine;

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
