using UnityEngine;

public class MakeBreakfastTask : SimpleTask
{   
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
}
