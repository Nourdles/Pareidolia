using UnityEngine;

public abstract class SimpleTask: Task
{
    protected void completeTask()
    {
        Debug.Log("Completing Task...");
        complete = true;
    }
}
