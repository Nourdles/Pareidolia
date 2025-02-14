using UnityEngine;

public abstract class SimpleTask: Task
{
    protected virtual void completeTask()
    {
        Debug.Log("Completing Simple Task...");
        complete = true;
    }
}
