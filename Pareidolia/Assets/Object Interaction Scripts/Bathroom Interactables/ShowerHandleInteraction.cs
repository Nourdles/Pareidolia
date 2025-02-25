using UnityEngine;

public class TubInteraction : ObjectInteraction // or tub interaction
{
    public override void interact(GameObject objectInHand)
    {
        // if holding something, then say "I can't go in the shower with this"
        // else
        // move player into tub (fade to black?) + reclose the curtains
        // invoke some event to enable Shower script
        // also listen to the shower completion event, set this object to uninteractable
        throw new System.NotImplementedException();
    }

}
