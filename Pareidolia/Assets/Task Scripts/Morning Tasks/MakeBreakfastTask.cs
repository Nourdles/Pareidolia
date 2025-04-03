using UnityEngine;

public class MakeBreakfastTask : SimpleTask
{   
    [SerializeField] private FMODEvents fmodEvents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.MakeBreakfast;
        stringrepresentation = "make breakfast";
    }

    void OnEnable()
    {
        BowlInteraction.BreakfastMadeEvent += completeTask;
    }

    void OnDisable()
    {
        BowlInteraction.BreakfastMadeEvent -= completeTask;
    }

    private void completeTask()
    {
        // Trigger FMOD to update the "Task Level" parameter to 1
        if (fmodEvents != null)
        {
            Debug.Log("MakeBreakfast task completed! Updating Task Level to 1.");
            fmodEvents.UpdateTaskLevel(1); // Set Task Level to 1
        }
    }
}
