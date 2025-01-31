using System;
using UnityEngine;

public class CleanBed: SimpleTask
{
    protected override void completeTask()
    {
        base.completeTask();
        // must invoke task completion event for TaskManager
    }

    private void OnEnable() 
    {
        // subscribe to taskobject delegate
    }


    private void OnDisable() 
    {
        // unsubscribe to taskobject delegate
    }
}
