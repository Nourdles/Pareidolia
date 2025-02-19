using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    //public static event Action DropEvent;

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        } else
        {
            PickUpEvent?.Invoke(gameObject);
        }
    }
}
