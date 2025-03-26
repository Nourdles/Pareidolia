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
        PreventClipping();
    }

    public void PreventClipping()
    {
        //Debug.Log("Preventing Clipping");
        GameObject objectCenter = FindObjectCenter();
        var clipRange = Vector3.Distance(gameObject.transform.position, playerCam.transform.position * 1.5f); //distance from holdPos/the held object to the camera (offset so the ray doesn't start from inside collider)

        RaycastHit[] rayHits;
        rayHits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), clipRange);
        Debug.Log("Clip Range:" + clipRange);
        Debug.Log(rayHits);
        // check if the raycasts have detected another object between the object hold position and camera
        if (rayHits.Length > 1)
        {
            Debug.Log("Clipping Detected");
            Debug.Log("Clip Range:" + clipRange);
            // move object to be positioned slightly below player camera so it doesn't clip upon dropping
            objectCenter.transform.position = playerCam.transform.position + new Vector3(0f, -0.1f, 0f);
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
