using System;
using UnityEngine;


/// <summary>
/// Logic for interaction with the washing machine BODY
/// </summary>
public class LaundryMachineInteraction : ObjectInteraction
{
    private bool _doorOpen = false;
    public static event Action ClothesPutInWasherEvent;
    public override void interact(GameObject objectInHand)
    {
        // if door not open, open it
        // else if door open, proceed with below
        if (objectInHand != null)
        {
            HandheldObjectInteraction objectInteraction = objectInHand.GetComponent<HandheldObjectInteraction>();
            Handhelds handheld_id = objectInteraction.getHandheld();
            if (handheld_id == Handhelds.LaundryBin)
            {
                if (((LaundryBinInteraction)objectInteraction).GetIsFull())
                {
                    // if yes: put laundry in machine + sfx
                    ClothesPutInWasherEvent?.Invoke();
                    
                } else
                {
                    InvokeDialoguePromptEvent("I haven't picked up all my dirty clothes yet    " + 
                        ((LaundryBinInteraction)objectInteraction).GetNumMissing());
                }
            } else
            {
                InvokeDialoguePromptEvent("I can't put this in the washing machine!");
            }
        } else
        {
            InvokeDialoguePromptEvent("I need to wash my dirty clothes");
        }
    }
}
