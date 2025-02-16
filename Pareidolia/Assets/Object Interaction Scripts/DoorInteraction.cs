using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : ObjectInteraction
{
    public Animator doorAnimator;

    public event Action DoorFirstOpeningEvent;
    public event Action DoorUnlockEvent;

    public bool locked = true;
    private bool firstOpen = true;
    private bool doorOpen = false;


    protected override void Start()
    {
        base.Start();
        //doorAnimator = gameObject.GetComponent<Animator>();
    }

    
    protected override void Update()
    {
        
        if (CanInteract() && !locked)
        {
            if (firstOpen)
            {
                DoorAnimation();
                InvokeInteractionEvent();
            
            } else
            {
                DoorAnimation();
            }
        }
    }

    protected override void InvokeInteractionEvent()
    {
        DoorFirstOpeningEvent?.Invoke();
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
