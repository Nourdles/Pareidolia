using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    New class to switch interaction tracking to player instead of object.
*/
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject objectInView; // the object the player is looking at
    [SerializeField] private Transform objectHoldPointTransform;
    private InputAction interactKey;
    private InventoryManager playerInventory;
    public static event Action DropItemEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
        playerInventory = gameObject.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectInView != null && interactKey.WasPressedThisFrame()) // looking at object
        {
            GameObject objectInHand = playerInventory.getHandheld();
            objectInView.GetComponent<ObjectInteraction>().interact(objectInHand);
        } else if (interactKey.WasPressedThisFrame() && playerInventory.isHoldingObject())
        {
            DropItemEvent?.Invoke();
        }
    }

    private void SetObjectInView(GameObject gameObject) 
    {
        objectInView = gameObject;
    }

    private void PickUp(GameObject handheld)
    {
        Debug.Log("Picking up an item");
        handheld.GetComponent<HandheldObjectInteraction>().HoldObject(objectHoldPointTransform);
        SetObjectInView(null);
    }

    public GameObject GetObjectInView()
    {
        return objectInView;
    }

    void OnEnable()
    {
        ObjectHoverGlow.ViewingObjectEvent += SetObjectInView;
        HandheldObjectInteraction.PickUpEvent += PickUp;
    }

    void OnDisable()
    {
        ObjectHoverGlow.ViewingObjectEvent -= SetObjectInView;
        HandheldObjectInteraction.PickUpEvent -= PickUp;
    }
}
