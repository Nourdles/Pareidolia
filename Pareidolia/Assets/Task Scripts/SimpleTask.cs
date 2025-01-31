using UnityEngine;

public abstract class SimpleTask: Task
{
    protected virtual void completeTask()
    {
        Debug.Log("Completing Task...");
        complete = true;
    }
}
