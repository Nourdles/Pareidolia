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

    private Camera playerCam;

    protected override void Start() 
    {
        base.Start();
        playerCam = GameObject.Find("Player Camera").GetComponent<Camera>(); // get the player camera (used to detect object clipping)

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

    protected override void interactaction(GameObject objectInHand)
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
        // set the object and its children's layer so that it can be rendered by the pickup camera
        SetLayerRecursively(gameObject, handheldLayer);
        Debug.Log("Layer set");

    }

    private void SetLayerRecursively(GameObject obj, int newLayer) // recursively apply layer ot gameobject's children
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public void DropObject()
    {
        itemRb.transform.parent = null;
        itemRb.isKinematic = false;
        itemRb.detectCollisions = true;
        // set as interactable again
        gameObject.tag = "InteractableObject";

        // revert this object and all children back to the default layer
        SetLayerRecursively(gameObject, defaultLayer);
        PreventClipping();
    }

    public void PreventClipping()
    {
        GameObject objectCenter = FindObjectCenter();

        Vector3 camForward = playerCam.transform.forward;
        camForward.y = Mathf.Max(camForward.y, -0.5f);
        Vector3 dropDir = camForward.normalized;

        Vector3 dropPos = playerCam.transform.position + dropDir * 1.5f + Vector3.down * 0.9f;
        Vector3 upwardDrop = dropPos + Vector3.up * 0.5f;
        Vector3 fallbackDrop = playerCam.transform.position + Vector3.down * 0.4f;

        int ignorePlayerAndHandheld = ~(LayerMask.GetMask("Player", "HandheldObjects"));

        // 1. Try front drop
        if (!Physics.CheckSphere(dropPos, 0.2f, ignorePlayerAndHandheld))
        {
            objectCenter.transform.position = dropPos;
            ResetDropVelocity();
            return;
        }

        // 2. Try slightly above
        if (!Physics.CheckSphere(upwardDrop, 0.2f, ignorePlayerAndHandheld))
        {
            objectCenter.transform.position = upwardDrop;
            ResetDropVelocity();
            return;
        }

        // 3. FINAL fallback: drop straight down & kill motion IMMEDIATELY
        itemRb.linearVelocity = Vector3.zero;
        itemRb.angularVelocity = Vector3.zero;
        itemRb.MovePosition(fallbackDrop); // use MovePosition to bypass weird bounce force
    }

    private void ResetDropVelocity()
    {
        if (itemRb != null)
        {
            itemRb.linearVelocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;
        }
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
