using UnityEngine;

public class WashLaundry : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.WashLaundry;
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
