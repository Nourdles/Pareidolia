using UnityEngine;
using System;
using UnityEngine.InputSystem;
using FMODUnity;

public class MilkInteraction : HandheldObjectInteraction
{
    public static event Action MilkPickupEvent;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Milk;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup milk";
        task = taskManager.GetComponentInChildren<MakeBreakfastTask>();
    }

    protected override void InvokePickupEvent()
    {
        MilkPickupEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup milk";
    }
}
