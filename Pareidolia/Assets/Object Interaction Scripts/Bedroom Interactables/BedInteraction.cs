using System;
using UnityEngine;

public class BedInteraction: ObjectInteraction
{
    private bool hasNotepad;
    public static event Action BedInteractionEvent;

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
