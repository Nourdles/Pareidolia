using UnityEngine;

// Must be tagged with "Untagged" first
public class ShowerMatInteraction : ObjectInteraction
{
    private bool _canLeave = false;
    
    public override void interact(GameObject objectInHand)
    {
        if (_canLeave)
        {
            // move player position to the mat
            SetUninteractable();
        } else
        {
            InvokeDialoguePromptEvent("I'm not done showering yet");
        }
    }


    private void SetCanLeave()
    {
        _canLeave = true;
    }

    // allows player to leave shower
    void OnEnable()
    {
        ShowerTask.ShowerComplete += SetCanLeave;
        TubInteraction.GetIntoTubEvent += SetInteractable;
    }

    void OnDisable()
    {
        ShowerTask.ShowerComplete -= SetCanLeave;
        TubInteraction.GetIntoTubEvent -= SetInteractable;
    }
}
