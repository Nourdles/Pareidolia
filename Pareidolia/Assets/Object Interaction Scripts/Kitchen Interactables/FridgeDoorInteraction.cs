using UnityEngine;

public class FridgeDoorInteraction : ObjectInteraction
{
    //public EventReference doorOpenSound;
    //public EventReference doorCloseSound;
    private bool doorOpen = false;
    [SerializeField] Animator doorAnimator;


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
            //AudioManager.instance.PlayOneShot(doorCloseSound, this.transform.position);
        }
        else
        {
            doorAnimator.Play("OpenFridgeDoor");
            Debug.Log("Door Opening");
            //AudioManager.instance.PlayOneShot(doorOpenSound, this.transform.position);
        }
        doorOpen = !doorOpen;
    }
}
