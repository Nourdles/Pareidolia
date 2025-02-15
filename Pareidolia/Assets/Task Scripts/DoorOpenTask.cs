using UnityEngine;

public class DoorOpenTask : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int tasknum = 2;
    private void OnEnable()
    {
        DoorInteraction.DoorInteractionEvent += completeTask;
    }

    private void OnDisable()
    {
        DoorInteraction.DoorInteractionEvent -= completeTask;
    }

    protected override void completeTask()
    {
        base.completeTask();
        invokeCompleteTaskEvent(tasknum);
    }
}
