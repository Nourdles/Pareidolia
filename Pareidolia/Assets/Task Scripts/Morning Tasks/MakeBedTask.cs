using System;
using System.Threading.Tasks;
using UnityEngine;

public class MakeBedTask: SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) TutorialTasks.MakeBed;
        _stringrepresentation = "make my bed";
    }
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += completeTask;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= completeTask;
    }
}
