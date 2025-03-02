using System;
using UnityEngine;
using FMODUnity;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    [SerializeField] protected Handhelds handheld_id;
    [SerializeField] private Rigidbody itemRb;
    
    // FMOD event for object pickup sounds
    [SerializeField] protected FMODUnity.EventReference pickupSFX;

    private int handheldLayer;
    private int defaultLayer;

    protected override void Start() 
    {
        base.Start();
        itemRb = gameObject.GetComponentInParent<Rigidbody>();
        handheldLayer = LayerMask.NameToLayer("HandheldObjects");
        defaultLayer = LayerMask.NameToLayer("Default");
    }

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        } else
        {
            AudioManager.instance.PlayOneShot(pickupSFX, this.transform.position); //Trigger the sfx

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
        // set the objects layer so that it can be rendered by the pickup camera
        gameObject.layer = handheldLayer;
        Debug.Log("Layer set");

    }

    public void DropObject()
    {
        itemRb.transform.parent = null;
        itemRb.isKinematic = false;
        itemRb.detectCollisions = true;
        // set as interactable again
        gameObject.tag = "InteractableObject";

        // set the object's layer back to default
        gameObject.layer = defaultLayer;

    }

    /*

    void OnEnable()
    {
        PlayerInteract.DropItemEvent += DropObject;
    }

    void OnDisable()
    {
        PlayerInteract.DropItemEvent -= DropObject;
    }
    */
}
