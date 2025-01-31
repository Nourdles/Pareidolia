using System;
using UnityEngine;

public class MakeBedTask: SimpleTask
{
    public static event Action CompleteMakeBedTask;
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += completeTask;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= completeTask;
    }

    protected override void completeTask()
    {
        base.completeTask();
        CompleteMakeBedTask?.Invoke();
    }

}
