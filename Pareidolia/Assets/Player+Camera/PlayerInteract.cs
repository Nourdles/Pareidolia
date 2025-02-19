using UnityEngine;
using UnityEngine.InputSystem;

/*
    New class to switch interaction tracking to player instead of object.
*/
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject objectInView; // the object the player is looking at
    private InputAction interactKey;
    private InventoryManager playerInventory;
    
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
            // drop the current item
        }
    }

    private void SetObjectInView(GameObject gameObject) 
    {
        objectInView = gameObject;
    }

    public GameObject GetObjectInView()
    {
        return objectInView;
    }

    void OnEnable()
    {
        PlayerView.ViewingObjectEvent += SetObjectInView;
    }

    void OnDisable()
    {
        PlayerView.ViewingObjectEvent -= SetObjectInView;
    }
}
