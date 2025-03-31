using UnityEngine;

public class MakeBreakfastTask : SimpleTask
{   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.MakeBreakfast;
        _stringrepresentation = "make breakfast";
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
