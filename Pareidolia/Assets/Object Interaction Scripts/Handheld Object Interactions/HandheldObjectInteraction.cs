using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    private Rigidbody itemRb;
    //public static event Action DropEvent;

    protected override void Start() 
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
        gameObject.transform.rotation = objectHoldPointTransform.rotation;
        

        itemRb.transform.parent = objectHoldPointTransform.transform;
        itemRb.isKinematic = true;
        itemRb.detectCollisions = false;

        // set the object tag as untagged so it can't be interacted with
        gameObject.tag = "Untagged";

    }

    public void DropObject()
    {
        itemRb.transform.parent = null;
        itemRb.isKinematic = false;
        itemRb.detectCollisions = true;
        // set as interactable again
        gameObject.tag = "InteractableObject";
    }

    void OnEnable()
    {
        PlayerInteract.DropItemEvent += DropObject;
    }

    void OnDisable()
    {
        PlayerInteract.DropItemEvent -= DropObject;
    }
}
