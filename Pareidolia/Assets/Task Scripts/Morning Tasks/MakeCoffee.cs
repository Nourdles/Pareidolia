using UnityEngine;

public class MakeCoffee : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) MorningTasks.MakeCoffee;
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
