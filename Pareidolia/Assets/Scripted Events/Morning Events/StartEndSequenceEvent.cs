using UnityEngine;

/// <summary>
/// Once last task (shower task) is finished, open the front door.
/// </summary>
public class StartEndSequenceEvent : MonoBehaviour
{
    [SerializeField] private DoorInteraction morningFrontDoor;
    [SerializeField] private GameObject endHallway;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        endHallway.SetActive(false);
    }
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
        // remove the front door (will be replaced by the door in the end hallway sequence
        //morningFrontDoor.gameObject.SetActive(false);
        // load in the end hallway additively
        //SceneSwitcher.LoadSceneOnTop("EndSequence");

        // unlock front door, reveal hallway
        Debug.Log("Front door unlocked");
        morningFrontDoor.locked = false;
        endHallway.SetActive(true);


        // play some noises, add stains on ground leading to front door?

    }

}
