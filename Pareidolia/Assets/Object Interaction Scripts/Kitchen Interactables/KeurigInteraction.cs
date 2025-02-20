using System;
using UnityEngine;

public class KeurigInteraction : ObjectInteraction
{
    [SerializeField] private Transform cupHoldPointTransform;
    public static event Action CoffeeMadeEvent;
    //public static event Action CoffeeDrankEvent;
    public static event Action CupPutInMachineEvent;
    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            Handhelds handheld_id = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld_id == Handhelds.Cup)
            {
                PutCupInMachine(objectInHand);
                // invoke after x seconds (time for coffee to complete)
                CoffeeMadeEvent?.Invoke();
                gameObject.tag = "Untagged"; // no longer interactable

            } else
            {
                InvokeDialoguePromptEvent("I need to get my coffee mug for this");
            }
        } else
        {
            InvokeDialoguePromptEvent("I need to get my coffee mug for this");
        }
    }

    private void PutCupInMachine(GameObject cup)
    {
        cup.transform.position = cupHoldPointTransform.position;
        cup.transform.rotation = cupHoldPointTransform.rotation;
        
        Rigidbody cupRb = cup.GetComponent<Rigidbody>();
        cupRb.transform.parent = cupHoldPointTransform.transform;
        cupRb.isKinematic = false;
        cupRb.detectCollisions = true;
        // set as interactable again
        cup.tag = "InteractableObject";
        CupPutInMachineEvent?.Invoke();
    }
}
