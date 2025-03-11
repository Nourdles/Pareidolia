using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class CoffeeCupInteraction : HandheldObjectInteraction
{
    public static event Action CupPickupEvent;
    [SerializeField] private FMODUnity.EventReference mugPickupSFX;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cup;

        pickupSFX = mugPickupSFX;
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to pickup coffee mug";
    }

    protected override void InvokePickupEvent()
    {
        CupPickupEvent?.Invoke();
    }
}
