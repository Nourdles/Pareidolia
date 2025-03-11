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
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to pickup notepad";
    }
    public override void interact(GameObject objectInHand)
    {
        ResetInteractionText();
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(notepadPickupSound, this.transform.position);
    }
}
