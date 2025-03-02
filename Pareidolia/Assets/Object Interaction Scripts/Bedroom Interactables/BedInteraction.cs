using System;
using UnityEngine;
using FMODUnity;

public class BedInteraction: ObjectInteraction
{
    private bool hasNotepad;
    public static event Action BedInteractionEvent;
    public EventReference bedMakeSound;

    protected override void Start()
    {
        base.Start();
        hasNotepad = false;
    }

    public override void interact(GameObject objectInHand)
    {
        if (hasNotepad)
        {
            BedInteractionEvent?.Invoke();
            AudioManager.instance.PlayOneShot(bedMakeSound, this.transform.position);
        } else
        {
            InvokeDialoguePromptEvent("I should pick up the notepad first");
        }
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
