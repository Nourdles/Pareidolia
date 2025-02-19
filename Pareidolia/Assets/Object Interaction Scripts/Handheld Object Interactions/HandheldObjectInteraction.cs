using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    //public static event Action DropEvent;

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        } else
        {
            PickUpEvent?.Invoke(gameObject);
            PickUp();
        }
    }

    // make a fxn here to move the object
    private void PickUp()
    {
        // parent the object to the camera
        // gameObject.position = 
        // remove rigidbody
        // do not detect collisions
        // rigid body isKinematic to true
    }

    //protected abstract void MoveToCamView(); // to move the object to a set position
}
