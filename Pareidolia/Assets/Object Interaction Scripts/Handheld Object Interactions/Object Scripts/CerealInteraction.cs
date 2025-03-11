using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CerealInteraction : HandheldObjectInteraction
{
    public static event Action CerealPickupEvent;
    [SerializeField] private FMODUnity.EventReference cerealPickupSFX;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cereal;
        pickupSFX = cerealPickupSFX;
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to pickup cereal";
    }

    protected override void InvokePickupEvent()
    {
        CerealPickupEvent?.Invoke();
    }
}
