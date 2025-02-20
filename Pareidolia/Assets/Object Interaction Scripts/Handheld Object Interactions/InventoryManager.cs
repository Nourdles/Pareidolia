using UnityEngine;

/* Manages the handheld object inventory of the player. Attach to player*/
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject objectInHand; // reference to object being held


    void OnEnable()
    {
        HandheldObjectInteraction.PickUpEvent += pickupObject;
        // HandheldObjectInteraction.DropEvent += dropObject;
    }

    void OnDisable()
    {
        HandheldObjectInteraction.PickUpEvent -= pickupObject;
        // HandheldObjectInteraction.DropEvent -= dropObject;
    }

    private void pickupObject(GameObject objectToHold)
    {
        objectInHand = objectToHold;
    }

    private void dropObject()
    {
        objectInHand = null;
    }

    public bool isHoldingObject()
    {
        return objectInHand != null;
    }

    public GameObject getHandheld()
    {
        return objectInHand;
    }
}
