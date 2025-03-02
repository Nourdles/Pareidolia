using System;
using System.Threading.Tasks;
using UnityEngine;

public class MakeBedTask: SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = Tasks.MakeBed;
    }
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += completeTask;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= completeTask;
    }
}
