using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaundryDetergentInteraction : HandheldObjectInteraction
{
    public static event Action PickupDetergentEvent;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Detergent;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up laundry detergent";
        task = taskManager.GetComponentInChildren<WashLaundry>();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up laundry detergent";
    }
    
    protected override void InvokePickupEvent()
    {
        PickupDetergentEvent?.Invoke();
    }
}
