using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Logic for interaction with the washing machine BODY
/// </summary>
public class LaundryMachineInteraction : ObjectInteraction
{
    private bool _soapAdded = false;
    private bool _clothesAdded = false;
    private String instruction = "";
    public static event Action DoLaundryEvent;

    [SerializeField] private FMODUnity.EventReference washingMachineSFX;
    [SerializeField] private FMODUnity.EventReference detergentPourSFX;
    [SerializeField] private FMODUnity.EventReference clothingAddSFX;
    [SerializeField] private GameObject washingMachineContent; // clothes inside
    [SerializeField] private LaundrySpinRotator spinRotator; // rotator

    protected override void Start()
    {
        base.Start();
        task = taskManager.GetComponentInChildren<WashLaundry>();
    }

    protected override void interactaction(GameObject objectInHand)
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
                    ((LaundryBinInteraction)objectInteraction).HideBasketShirts(); // call to empty basket

                    AudioManager.instance.PlayOneShot(clothingAddSFX, transform.position);
                    if (washingMachineContent != null) // display the clothing inside
                    {
                        washingMachineContent.SetActive(true);
                    }
                    if (!_soapAdded)
                    {
                        InvokeDialoguePromptEvent("Now I just need to add detergent");
                    } else
                    {
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
                    AudioManager.instance.PlayOneShot(detergentPourSFX, transform.position);
                    if (_clothesAdded)
                    {
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
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to place clothes";
        instruction = "\"> to place clothes";
    }

    private void HoldingDetergentInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to add detergent";
        instruction = "\"> to add detergent";
    }

    private void StartLoad()
    {
        // play washing machine sound
        AudioManager.instance.PlayOneShot(washingMachineSFX, transform.position);
        DoLaundryEvent?.Invoke();
        SetUninteractable();
        
        if (spinRotator != null)
        {
            spinRotator.StartSpinning();
        }
    }

    protected override void AssignTask()
    {
        task = taskManager.GetComponentInChildren<WashLaundry>();
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
