using UnityEngine;
using System;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected bool complete;
    [SerializeField] protected Task next;
    protected int task;
    protected static string _stringrepresentation;
    [SerializeField] protected bool active;
    public static event Action<int> CrossOutTaskEvent;
    public static event Action CompleteTaskEvent;
    
    protected virtual void Start()
    {
        complete = false;
        active = false;
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
        active = false;
        invokeCompleteTaskEvent((int) task);
        enabled = false; // disable the task
    }

    public Task GetNextTask()
    {
        return next;
    }

    public void SetAsCurrent()
    {
        active = true;
    }

    public int GetTasknum()
    {
        return task;
    }

    public bool GetActiveStatus()
    {
        return active;
    }

    public override string ToString()
    {
        return _stringrepresentation;
    }
}
