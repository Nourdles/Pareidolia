using UnityEngine;
using System;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected bool complete;
    protected int task;
    public static event Action<int> CrossOutTaskEvent;
    public static event Action CompleteTaskEvent;
    
    protected virtual void Start()
    {
        complete = false;
    }
    
    protected virtual void invokeCompleteTaskEvent(int tasknum)
    {
        CrossOutTaskEvent?.Invoke(tasknum);
        CompleteTaskEvent?.Invoke();
    }

    public bool isCompleted()
    {
        return complete;
    }

    protected void completeTask()
    {
        complete = true;
        invokeCompleteTaskEvent((int) task);
        enabled = false; // disable the task
    }
}
