using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class CerealInteraction : HandheldObjectInteraction
{
    public static event Action CerealPickupEvent;

    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cereal;

        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup cereal";
        task = taskManager.GetComponentInChildren<MakeBreakfastTask>();
    }

    protected override void InvokePickupEvent()
    {
        CerealPickupEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup cereal";
    }
}