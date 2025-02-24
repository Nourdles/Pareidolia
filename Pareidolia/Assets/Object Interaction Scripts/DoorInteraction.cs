using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class DoorInteraction : ObjectInteraction
{
    [SerializeField] private Animator doorAnimator;

    public event Action DoorFirstOpeningEvent;
    public event Action DoorUnlockEvent;
    [SerializeField] EventReference doorOpenSound;
    [SerializeField] EventReference doorCloseSound;
    [SerializeField] EventReference doorLockSound;

    public bool locked = true;
    private bool firstOpen = true;
    private bool doorOpen = false;


    protected override void Start()
    {
        base.Start();
        //doorAnimator = gameObject.GetComponent<Animator>();
    }

    public override void interact(GameObject objectInHand)
    {
        if (!locked)
        {
            if (firstOpen)
            {
                DoorAnimation();
                DoorFirstOpeningEvent?.Invoke();
                firstOpen = false;
            
            } else
            {
                DoorAnimation();
            }
        } else
        {
            InvokeDialoguePromptEvent("I shouldn't leave till I make my bed");
            AudioManager.instance.PlayOneShot(doorLockSound, this.transform.position);
        }
    }


    private void DoorAnimation()
    {
        if (doorOpen)
        {
            doorAnimator.Play("DoorClose");
            Debug.Log("Door Closing");
            AudioManager.instance.PlayOneShot(doorCloseSound, this.transform.position);
        }
        else
        {
            doorAnimator.Play("DoorOpen");
            Debug.Log("Door Opening");
            AudioManager.instance.PlayOneShot(doorOpenSound, this.transform.position);
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
