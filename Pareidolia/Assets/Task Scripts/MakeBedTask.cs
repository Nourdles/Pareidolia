using System;
using UnityEngine;

public class MakeBedTask: SimpleTask
{
    private int tasknum = 1;
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += completeTask;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= completeTask;
    }

    protected override void completeTask()
    {
        base.completeTask();
        invokeCompleteTaskEvent(tasknum);
    }

}
