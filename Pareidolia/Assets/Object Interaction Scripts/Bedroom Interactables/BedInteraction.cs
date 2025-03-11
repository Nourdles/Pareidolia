using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

public class BedInteraction: ObjectInteraction
{
    private bool hasNotepad;
    public static event Action BedInteractionEvent;
    public EventReference bedMakeSound;

    protected override void Start()
    {
        base.Start();
        hasNotepad = false;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to make the bed";
    }

    public override void interact(GameObject objectInHand)
    {
        if (hasNotepad)
        {
            ResetInteractionText();
            AudioManager.instance.PlayOneShot(bedMakeSound, this.transform.position);
            BedInteractionEvent?.Invoke();
        } else
        {
            InvokeDialoguePromptEvent("I should pick up the notepad first");
        }
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to make the bed";
    }

    private void setHasNotepad()
    {
        hasNotepad = true;
    }

    void OnEnable()
    {
        NoteInteraction.NotepadPickedUp += setHasNotepad;
    }

    void OnDisable()
    {
        NoteInteraction.NotepadPickedUp -= setHasNotepad;
    }
}
