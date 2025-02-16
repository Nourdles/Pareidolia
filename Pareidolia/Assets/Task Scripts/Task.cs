using UnityEngine;
using System;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected bool complete;
    public static event Action<int> CompleteTaskEvent;
    
    void Start()
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
}
