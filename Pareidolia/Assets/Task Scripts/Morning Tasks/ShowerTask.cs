using System;
using UnityEngine;

public class ShowerTask : ProgressTask
{
    public static event Action ShowerComplete;

    protected override void Start()
    {
        base.Start();
        task = (int) MorningTasks.Shower;
    }

    protected override void invokeCompleteTaskEvent(int tasknum)
    {
        base.invokeCompleteTaskEvent(tasknum);
        ShowerComplete?.Invoke();
    }

    void OnEnable()
    {
        Shower.ShowerOnEvent += startCharging;
        Shower.ShowerOffEvent += stopCharging;
    }

    void OnDisable()
    {
        Shower.ShowerOnEvent -= startCharging;
        Shower.ShowerOffEvent -= stopCharging;
    }
}
