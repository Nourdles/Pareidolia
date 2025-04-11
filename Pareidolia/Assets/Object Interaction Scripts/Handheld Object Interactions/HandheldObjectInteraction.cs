using System;
using UnityEngine;
using FMODUnity;
using System.Linq;
using System.Collections.Generic;

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
        itemRb.detectCollisions = true;
        PreventClipping();
        itemRb.transform.parent = null;
        itemRb.isKinematic = false;

        // set as interactable again
        gameObject.tag = "InteractableObject";

        // revert this object and all children back to the default layer
        SetLayerRecursively(gameObject, defaultLayer);

    }

    public void PreventClipping()
    {
        //Debug.Log("Preventing Clipping");
        GameObject objectCenter = FindObjectCenter();
        Vector3 rayStart = playerCam.transform.position + playerCam.transform.forward * 0.03f;

        var clipRange = Vector3.Distance(gameObject.transform.position, rayStart) * 1.3f; //distance from holdPos/the held object to the camera (offset so the ray doesn't start from inside collider)
        Vector3 directionToObject = (gameObject.transform.position - playerCam.transform.position).normalized;

        List<RaycastHit> rayList = new List<RaycastHit>();
        rayList.AddRange(Physics.RaycastAll(rayStart, playerCam.transform.TransformDirection(Vector3.forward), clipRange));
        rayList.AddRange(Physics.RaycastAll(playerCam.transform.position, directionToObject, clipRange));

        RaycastHit[] rayHits = rayList.ToArray();
        Debug.Log(rayHits);


        // check if the raycasts have detected another object between the object hold position and camera
        if (rayHits.Length > 1)
        {
            bool foundHit = false;
            foreach (RaycastHit hit in rayHits)
            {
                if (hit.collider != null && !hit.collider.transform.IsChildOf(gameObject.transform))
                {
                    Debug.Log("SEB: " + "hit object " + hit.collider.name);
                    foundHit = true;
                    break;
                }
            }
            if (!foundHit) { return; }
                //Vector3 moveDirection = hit.normal;  // Surface normal
                //float moveStep = 0.1f;  // Step size for moving the object
                //Vector3 newPosition = objectCenter.transform.position + moveDirection * moveStep;

                //// Gradually move the object until you find a clear space
                //while (Physics.Raycast(newPosition, Vector3.down, 0.5f))  // Check if the new position is still blocked
                //{
                //    newPosition += moveDirection * moveStep;  // Keep moving the object
                //}

                //// Move object to clear position
                //objectCenter.transform.position = newPosition;
                //Debug.Log("Moved object to clear space.");
            //}
            Collider objectCollider = gameObject.GetComponent<MeshCollider>();
            if (objectCollider != null)
            {
                Vector3 offset = new Vector3(0f, (-objectCollider.bounds.extents.y), 0f);
                objectCenter.transform.position = playerCam.transform.position + offset;
            }
            else
            {
                // move object to be positioned slightly below player camera so it doesn't clip upon dropping
                objectCenter.transform.position = playerCam.transform.position + new Vector3(0f, -0.1f, 0f);
            }

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
