using System;
using UnityEngine;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    [SerializeField] private Rigidbody itemRb;

    protected override void Start() 
    {
        base.Start();
        itemRb = gameObject.GetComponentInParent<Rigidbody>();
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

    private GameObject FindObjectCenter()
{
   Transform t = gameObject.transform;
   while (t.parent != null)
   {
      if (t.parent.tag == "HandheldCenter")
      {
         return t.parent.gameObject;
      }
      t = t.parent.transform;
   }
   return null; // Could not find a parent with given tag.
}

    public Handhelds getHandheld()
    {
        return handheld_id;
    }

    public void HoldObject(Transform objectHoldPointTransform)
    {
        itemRb.transform.parent = objectHoldPointTransform.transform;
        itemRb.isKinematic = true;
        itemRb.detectCollisions = false;
        
        GameObject objectCenter = FindObjectCenter();
        objectCenter.transform.localPosition = Vector3.zero;
        objectCenter.transform.localRotation = Quaternion.identity;

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
