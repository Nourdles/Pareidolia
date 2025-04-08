using UnityEngine;

public class MakeCoffeeTask : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.MakeCoffee;
        stringrepresentation = "make coffee";
    }

    void OnEnable()
    {
        KeurigInteraction.CoffeeMadeEvent += completeTask;
    }

    void OnDisable()
    {
        KeurigInteraction.CoffeeMadeEvent -= completeTask;
    }
}
