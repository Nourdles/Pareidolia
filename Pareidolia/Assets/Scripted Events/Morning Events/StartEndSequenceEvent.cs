using UnityEngine;

/// <summary>
/// Once last task (shower task) is finished, open the front door.
/// </summary>
public class StartEndSequenceEvent : MonoBehaviour
{
    [SerializeField] private DoorInteraction frontDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnEnable()
    {
        LaundryMachineInteraction.DoLaundryEvent += StartEvent;
    }

    public void OnDisable()
    {
        LaundryMachineInteraction.DoLaundryEvent -= StartEvent;
    }

    /// <summary>
    /// Unlock the front door and lead the player to it. 
    /// </summary>
    private void StartEvent()
    {
        // unlock the front door
        frontDoor.UnlockDoor();


        // play some noises, add stains on ground leading to front door?

    }

}
