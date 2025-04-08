using UnityEngine;

/* Manages the handheld object inventory of the player. Attach to player*/
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject objectInHand = null; // reference to object being held

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

    private void DisableHandheld()
    {
        if (objectInHand != null)
        {
            objectInHand.SetActive(false);
        }
    }

    private void EnableHandheld()
    {
        if (objectInHand != null)
        {
            objectInHand.SetActive(true);
        }
    }  

    private void DropItemOnDeath()
    {
        if (objectInHand != null)
        {
            objectInHand.GetComponent<HandheldObjectInteraction>().DropObject();
            dropObject();
        }
    }

       void OnEnable()
    {
        HandheldObjectInteraction.PickUpEvent += pickupObject;
        PlayerInteract.DropItemEvent += dropObject;
        KeurigInteraction.CupPutInMachineEvent += dropObject;
        DeathManager.DeathSceneEvent += DropItemOnDeath;
        SofaInteraction.TVStartEvent += DisableHandheld;
        TVSceneManager.TVWatchedEvent += EnableHandheld;
    }

    void OnDisable()
    {
        HandheldObjectInteraction.PickUpEvent -= pickupObject;
        PlayerInteract.DropItemEvent -= dropObject;
        KeurigInteraction.CupPutInMachineEvent -= dropObject;
        DeathManager.DeathSceneEvent -= DropItemOnDeath;
        SofaInteraction.TVStartEvent -= DisableHandheld;
        TVSceneManager.TVWatchedEvent -= EnableHandheld;
    }
}
