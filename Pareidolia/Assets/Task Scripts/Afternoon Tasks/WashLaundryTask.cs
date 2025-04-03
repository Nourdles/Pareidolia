using UnityEngine;

public class WashLaundry : SimpleTask
{
    [SerializeField] private FMODEvents fmodEvents;
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.WashLaundry;
        stringrepresentation = "wash my laundry";
    }

    void OnEnable()
    {
        LaundryMachineInteraction.DoLaundryEvent += completeTask;
    }

    void OnDisable()
    {
        LaundryMachineInteraction.DoLaundryEvent -= completeTask;
    }
    private void completeTask()
    {
        // Update FMOD Task Level parameter to 3
        if (fmodEvents != null)
        {
            Debug.Log("WashLaundry task completed! Updating Task Level to 3.");
            fmodEvents.UpdateTaskLevel(3); // Set Task Level to 3
        }
    }
}
