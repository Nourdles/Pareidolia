using System;
using FMODUnity;

public class NoteInteraction : ObjectInteraction
{
    public EventReference notepadPickupSound;
    public static event Action NotepadPickedUp;

    protected override void InvokeInteractionEvent()
    {
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(notepadPickupSound, this.transform.position);
    }
}
