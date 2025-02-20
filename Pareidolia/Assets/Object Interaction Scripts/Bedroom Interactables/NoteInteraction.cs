using System;
using FMODUnity;
using UnityEngine;

public class NoteInteraction : ObjectInteraction
{
    public EventReference notepadPickupSound;
    public static event Action NotepadPickedUp;

    public override void interact(GameObject objectInHand)
    {
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(notepadPickupSound, this.transform.position);
    }
}
