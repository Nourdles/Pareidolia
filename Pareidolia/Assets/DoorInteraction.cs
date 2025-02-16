using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    private InputAction interactKey;
    private InteractionManager interactionManager;
    public Animator doorAnimator;

    public event Action DoorFirstOpeningEvent;
    public event Action DoorUnlockEvent;

    public bool locked = true;
    private bool firstOpen = true;
    private bool doorOpen = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        interactKey = InputSystem.actions.FindAction("Interact");
        //doorAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (interactionManager.checkIfInteractable() && !locked)
        {
            if (firstOpen && interactKey.WasPressedThisFrame())
            {
                // if this is the first time the player is opening the door, trigger event
                Debug.Log("Invoking interaction event");
                DoorAnimation();
                DoorFirstOpeningEvent?.Invoke();
            }
            else if (interactKey.WasPressedThisFrame())
            {
                // not first time the player has interacted with the door
                DoorAnimation();
            }

        }
    }


    private void DoorAnimation()
    {
        if (doorOpen)
        {
            doorAnimator.Play("DoorClose");
            Debug.Log("Door Closing");
        }
        else
        {
            doorAnimator.Play("DoorOpen");
            Debug.Log("Door Opening");
        }
        doorOpen = !doorOpen;
    }

    /*private void OnEnable()
    {
        BedInteraction.BedInteractionEvent += UnlockDoor;

    }

    private void OnDisable()
    {
        BedInteraction.BedInteractionEvent -= UnlockDoor;
    } */

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
