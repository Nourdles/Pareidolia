using UnityEngine;
using FMODUnity;
using System;

public class FridgeDoorInteraction : ObjectInteraction
{
    //public EventReference doorOpenSound;
    //public EventReference doorCloseSound;
    private bool doorOpen = false;
    private bool hasOpenedOnce = false;
    private bool hasClosedOnce = false;
    [SerializeField] Animator doorAnimator;
    [SerializeField] EventReference fridgeCloseSound;
    [SerializeField] EventReference fridgeOpenSound;
    public static event Action OnFirstFridgeOpen;
    public static event Action OnFirstFridgeClose;


    protected override void Start()
    {
        base.Start();
    }

    public override void interact(GameObject objectInHand)
    {
        DoorAnimation();
    }

    private void DoorAnimation()
    {
        if (doorOpen)
        {
            doorAnimator.Play("CloseFridgeDoor");
            Debug.Log("Door Closing");
            AudioManager.instance.PlayOneShot(fridgeCloseSound, this.transform.position);

            if (!hasClosedOnce)
            {
                hasClosedOnce = true;
                OnFirstFridgeClose?.Invoke();  // notify listeners
            }
        }
        else
        {
            doorAnimator.Play("OpenFridgeDoor");
            Debug.Log("Door Opening");
            AudioManager.instance.PlayOneShot(fridgeOpenSound, this.transform.position);

            if (!hasOpenedOnce)
            {
                hasOpenedOnce = true;
                OnFirstFridgeOpen?.Invoke();  // notify listeners
            }
        }
        doorOpen = !doorOpen;
    }
}
