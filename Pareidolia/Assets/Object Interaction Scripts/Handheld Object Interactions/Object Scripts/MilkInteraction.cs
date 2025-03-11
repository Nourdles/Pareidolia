using System;
using UnityEngine.InputSystem;

public class MilkInteraction : HandheldObjectInteraction
{
    public FMODUnity.EventReference milkPickupSFX;
    public static event Action MilkPickupEvent;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Milk;

        pickupSFX = milkPickupSFX;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup milk";
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
