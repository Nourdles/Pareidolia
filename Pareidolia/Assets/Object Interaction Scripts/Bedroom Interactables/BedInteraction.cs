using System;
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

    protected override void Update()
    {
        if (CanInteract())
        {
            if (hasNotepad)
            {
                InvokeInteractionEvent();
            } else
            {
                // right now this overlaps with the tutorial text
                InvokeDialoguePromptEvent("I should pick up the notepad first");
            }
        }
    }
    protected override void InvokeInteractionEvent()
    {
        // check if notepad has been picked up
        
        BedInteractionEvent?.Invoke();
        AudioManager.instance.PlayOneShot(bedMakeSound, this.transform.position);
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
