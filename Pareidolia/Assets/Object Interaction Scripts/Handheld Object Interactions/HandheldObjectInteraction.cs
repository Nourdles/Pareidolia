using System;
using UnityEngine;
using FMODUnity;

public abstract class HandheldObjectInteraction : ObjectInteraction
{
    public static event Action<GameObject> PickUpEvent;
    
    [SerializeField] protected Handhelds handheld_id;
    [SerializeField] private Rigidbody itemRb;
    
    // FMOD events for pickup & drop sounds
    [SerializeField] protected FMODUnity.EventReference pickupSFX;
    [SerializeField] protected FMODUnity.EventReference dropSFX;

    private int handheldLayer;
    private int defaultLayer;

    protected override void Start() 
    {
        base.Start();
        itemRb = gameObject.GetComponentInParent<Rigidbody>();
        handheldLayer = LayerMask.NameToLayer("HandheldObjects");
        defaultLayer = LayerMask.NameToLayer("Default");

        if (itemRb != null)
        {
            Debug.Log($"[Physics Debug] Rigidbody found: {itemRb.gameObject.name}");
            
            HandheadObjectCollisionListener collisionListener = itemRb.gameObject.AddComponent<HandheadObjectCollisionListener>();

            // Subscribe only to this object's listener
            collisionListener.OnObjectDropped += PlayDropSound;
        }
    }

    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            InvokeDialoguePromptEvent("My hands are full right now");
        }
        else
        {
            AudioManager.instance.PlayOneShot(pickupSFX, this.transform.position);
            PickUpEvent?.Invoke(gameObject);
            InvokePickupEvent();
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
        return null; 
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

    private void PlayDropSound(Vector3 position)
    {
        if (dropSFX.IsNull) {
            Debug.Log("Drop sound null");
        }
        RuntimeManager.PlayOneShot(dropSFX, transform.position);
    }

    protected abstract void InvokePickupEvent();
}
