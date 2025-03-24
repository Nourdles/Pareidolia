using System;
using UnityEngine.InputSystem;

public class SpoonInteraction : HandheldObjectInteraction
{
    public static event Action SpoonPickupEvent;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Spoon;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup spoon";
    }

    protected override void InvokePickupEvent()
    {
        SpoonPickupEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup spoon";
    }
}
