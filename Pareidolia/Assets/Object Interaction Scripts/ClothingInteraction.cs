using System;
using UnityEngine;

public class ClothingInteraction : ObjectInteraction
{
    public static event Action ClothingPickUpEvent;
    
    public override void interact(GameObject objectInHand)
    {
        // can only pick up if holding a bin
        if (objectInHand != null)
        {
            Handhelds handheld = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld == Handhelds.LaundryBin)
            {
                ClothingPickUpEvent?.Invoke();
            } else
            {
                InvokeDialoguePromptEvent("I should put these dirty clothes in the wash...I need to get my laundry bin from the washroom to pick these up");
            }
        } else
        {
            InvokeDialoguePromptEvent("I should put these dirty clothes in the wash...I need to get my laundry bin from the washroom to pick these up");
        }
    }
}
