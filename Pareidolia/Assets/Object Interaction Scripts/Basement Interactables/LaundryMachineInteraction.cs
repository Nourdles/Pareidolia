using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Logic for interaction with the washing machine BODY
/// </summary>
public class LaundryMachineInteraction : ObjectInteraction
{
    private bool _doorOpen = false;
    private bool _soapAdded = false;
    private bool _clothesAdded = false;
    private String instruction = "";
    public static event Action DoLaundryEvent;

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            HandheldObjectInteraction objectInteraction = objectInHand.GetComponent<HandheldObjectInteraction>();
            Handhelds handheld_id = objectInteraction.getHandheld();
            if (handheld_id == Handhelds.LaundryBin)
            {
                if (((LaundryBinInteraction)objectInteraction).GetIsFull())
                {
                    // if yes: put laundry in machine + sfx
                    _clothesAdded = true;
                    if (!_soapAdded)
                    {
                        InvokeDialoguePromptEvent("Now I just need to add detergent");
                    } else
                    {
                        if (_doorOpen)
                        {
                            // close door
                        }
                        StartLoad();
                    } 
                } else
                {
                    InvokeDialoguePromptEvent("I haven't picked up all my dirty clothes yet    " + 
                        ((LaundryBinInteraction)objectInteraction).GetNumMissing());
                }
            } else if (handheld_id == Handhelds.Detergent)
            {
                if (_soapAdded)
                {
                    InvokeDialoguePromptEvent("I already added soap");
                } else
                {
                    _soapAdded = true;
                    if (_clothesAdded)
                    {
                        if (_doorOpen)
                        {
                            // close door
                        }
                        StartLoad();
                    } else
                    {
                        InvokeDialoguePromptEvent("Now I just need to put my dirty clothes in");
                    }
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

    protected override void UpdateInteractText()
    {
        if (interactText != "")
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + instruction;
        }
    }

    private void HoldingBinInteractText()
    {
        interactText = interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to place clothes";
        instruction = "\"> to place clothes";
    }

    private void HoldingDetergentInteractText()
    {
        interactText = interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to add detergent";
        instruction = "\"> to add detergent";
    }

    private void StartLoad()
    {
        // play washing machine sound
        DoLaundryEvent?.Invoke();
        SetUninteractable();
    }

    void OnEnable()
    {
        PlayerInteract.DropItemEvent += ResetInteractionText;
        LaundryBinInteraction.PickupBinEvent += HoldingBinInteractText;
        LaundryDetergentInteraction.PickupDetergentEvent += HoldingDetergentInteractText;
    }

    void OnDisable()
    {
        PlayerInteract.DropItemEvent -= ResetInteractionText;
        LaundryBinInteraction.PickupBinEvent -= HoldingBinInteractText;
        LaundryDetergentInteraction.PickupDetergentEvent -= HoldingDetergentInteractText;
    }
}
