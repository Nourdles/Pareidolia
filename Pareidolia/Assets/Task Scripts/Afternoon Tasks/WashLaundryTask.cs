using UnityEngine;

public class WashLaundry : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) MorningTasks.Laundry;
    }

    void OnEnable()
    {
        LaundryMachineInteraction.DoLaundryEvent += completeTask;
    }

    void OnDisable()
    {
        LaundryMachineInteraction.DoLaundryEvent -= completeTask;
    }
}
