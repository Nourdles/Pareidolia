using UnityEngine;
using FMODUnity;
using System;
using UnityEngine.InputSystem;

public class FridgeDoorInteraction : ObjectInteraction
{
    //public EventReference doorOpenSound;
    //public EventReference doorCloseSound;
    private bool doorOpen = false;
    private bool hasOpenedOnce = false;
    private bool hasClosedOnce = false;
    [SerializeField] Animator doorAnimator;
    [SerializeField] EventReference fridgeCloseSound;
    [SerializeField] EventReference fridgeOpenSound;
    public static event Action OnFirstFridgeOpen;
    public static event Action OnFirstFridgeClose;


    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open fridge";
    }

    protected override void interactaction(GameObject objectInHand)
    {
        DoorAnimation();
    }

    private void DoorAnimation()
    {
        if (doorOpen)
        {
            doorAnimator.Play("CloseFridgeDoor");
            Debug.Log("Door Closing");
            AudioManager.instance.PlayOneShot(fridgeCloseSound, this.transform.position);

            if (!hasClosedOnce)
            {
                hasClosedOnce = true;
                OnFirstFridgeClose?.Invoke();  // notify listeners
            }

            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open fridge";
        }
        else
        {
            doorAnimator.Play("OpenFridgeDoor");
            Debug.Log("Door Opening");
            AudioManager.instance.PlayOneShot(fridgeOpenSound, this.transform.position);

            if (!hasOpenedOnce)
            {
                hasOpenedOnce = true;
                OnFirstFridgeOpen?.Invoke();  // notify listeners
            }

            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close fridge";
        }
        doorOpen = !doorOpen;
    }

    protected override void UpdateInteractText()
    {
        if (doorOpen)
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close fridge";
        } else
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open fridge";
        }
    }
}
