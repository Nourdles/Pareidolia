using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    //public static event Action DropEvent;

    protected override void InvokeInteractionEvent()
    {
        if (inventoryManager.isHoldingObject()) // hands full
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        } else // hands empty
        {
            PickUpEvent?.Invoke(gameObject);
        }
    }
    
}
