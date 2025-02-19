using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    private Rigidbody itemRb;
    //public static event Action DropEvent;

    override void Start() 
    {
        base.Start();
        itemRb = gameObject.GetComponent<Rigidbody>();
    }

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        } else
        {
            PickUpEvent?.Invoke(gameObject);
        }
    }

    public void HoldObject(Transform objectHoldPointTransform)
    {
        gameObject.transform.position = objectHoldPointTransform.position;
        itemRb.isKinematic = true;
        itemRb.detectCollisions = false;
    }

    public void DropObject()
    {
        
    }
}
