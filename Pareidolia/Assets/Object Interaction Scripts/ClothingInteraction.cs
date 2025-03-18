using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClothingInteraction : ObjectInteraction
{
    public static event Action ClothingPickUpEvent;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up dirty clothes";
    }

    public override void interact(GameObject objectInHand)
    {
        // can only pick up if holding a bin
        if (objectInHand != null)
        {
            Handhelds handheld = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld == Handhelds.LaundryBin)
            {
                ClothingPickUpEvent?.Invoke();
                Destroy(gameObject);
            } else
            {
                InvokeDialoguePromptEvent("I should put these dirty clothes in the wash...I need to get my laundry bin from the w=basement to pick these up");
            }
        } else
        {
            InvokeDialoguePromptEvent("I should put these dirty clothes in the wash...I need to get my laundry bin from the basement to pick these up");
        }
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up dirty clothes";
    }
}
