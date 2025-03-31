using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteInteraction : ObjectInteraction
{
    public EventReference notepadPickupSound;
    public static event Action NotepadPickedUp;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup notepad";
    }
    protected override void interactaction(GameObject objectInHand)
    {
        ResetInteractionText();
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(notepadPickupSound, this.transform.position);
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pickup notepad";
    }
}
