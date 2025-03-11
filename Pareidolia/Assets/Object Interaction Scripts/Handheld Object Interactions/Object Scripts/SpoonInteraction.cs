using System;
using UnityEngine.InputSystem;

public class SpoonInteraction : HandheldObjectInteraction
{
    public static event Action SpoonPickupEvent;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Spoon;
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to pickup spoon";
    }

    protected override void InvokePickupEvent()
    {
        SpoonPickupEvent?.Invoke();
    }
}
