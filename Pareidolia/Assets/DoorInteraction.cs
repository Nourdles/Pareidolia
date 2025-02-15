using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    private InputAction interactKey;
    private InteractionManager interactionManager;
    public static event Action DoorInteractionEvent;
    public static event Action DoorUnlockEvent;
    public bool locked = true;
    public bool firstInteract = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        interactKey = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionManager.checkIfInteractable() && !locked)
        {
            if (firstInteract && interactKey.WasPressedThisFrame())
            {
                // if this is the first time the player is opening the door, trigger event
                Debug.Log("Invoking interaction event");
                DoorInteractionEvent?.Invoke();
            }
            else if (interactKey.WasPressedThisFrame())
            {
                // not first time the player has interacted with the door
                // do something
            }
        }
    }


    public void UnlockDoor()
    {
        locked = false;
        Debug.Log("Door has been unlocked");
        DoorUnlockEvent?.Invoke();

    }

    public void LockDoor()
    {
        locked = true;
        Debug.Log("Door has been locked");

    }
}
