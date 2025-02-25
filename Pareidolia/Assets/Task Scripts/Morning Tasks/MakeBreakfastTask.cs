using UnityEngine;

public class MakeBreakfastTask : MultistepTask
{   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        task = Tasks.MakeBreakfast;
        numTasksRequired = 2; // make coffee, cereal
    }

    void OnEnable()
    {
        KeurigInteraction.CoffeeMadeEvent += completeSubTask;
        BowlInteraction.BreakfastMadeEvent += completeSubTask;
    }

    void OnDisable()
    {
        KeurigInteraction.CoffeeMadeEvent -= completeSubTask;
        BowlInteraction.BreakfastMadeEvent -= completeSubTask;
    }
}
