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
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup coffee mug";
        pickupSFX = mugPickupSFX;
    }

    protected override void InvokePickupEvent()
    {
        CupPickupEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup coffee mug";
    }
}
