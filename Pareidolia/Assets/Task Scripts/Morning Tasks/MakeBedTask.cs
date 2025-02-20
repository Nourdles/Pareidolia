using System;
using UnityEngine;

public class MakeBedTask: SimpleTask
{
    protected override void Start()
    {
        base.Start();
        tasknum = 1;
    }
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += completeTask;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= completeTask;
    }
}
