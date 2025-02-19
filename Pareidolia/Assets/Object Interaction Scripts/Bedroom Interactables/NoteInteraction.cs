using System;
using UnityEngine;

public class NoteInteraction : ObjectInteraction
{
    public static event Action NotepadPickedUp;

    public override void interact(GameObject objectInHand)
    {
        NotepadPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
