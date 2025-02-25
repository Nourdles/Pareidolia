using UnityEngine;
using System;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected bool complete;
    protected Tasks task;
    public static event Action<int> CompleteTaskEvent;
    
    protected virtual void Start()
    {
        complete = false;
    }
    
    protected void invokeCompleteTaskEvent(int tasknum)
    {
        CompleteTaskEvent?.Invoke(tasknum);
    }

    public bool isCompleted()
    {
        return complete;
    }

    protected void completeTask()
    {
        complete = true;
        invokeCompleteTaskEvent((int) task);
    }
}
