using System;

public class NoteInteraction : ObjectInteraction
{
    public static event Action NotepadPickedUp;

    protected override void InvokeInteractionEvent()
    {
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
