using System;
using UnityEngine;

public class ShowerTask : ProgressTask
{
    [SerializeField] private FMODEvents fmodEvents;
    public static event Action ShowerComplete;

    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.Shower;
        stringrepresentation = "take a shower";
    }

    protected override void invokeCompleteTaskEvent(int tasknum)
    {
        base.invokeCompleteTaskEvent(tasknum);
        ShowerComplete?.Invoke();

        // Update FMOD Task Level parameter to 2
        if (fmodEvents != null)
        {
            Debug.Log("Shower task completed! Updating Task Level to 2.");
            fmodEvents.UpdateTaskLevel(2); // Set Task Level to 2
        }
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
