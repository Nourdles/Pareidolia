using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// Once last task (shower task) is finished, open the front door.
/// </summary>
public class StartEndSequenceEvent : MonoBehaviour
{
    [SerializeField] private DoorInteraction morningFrontDoor;
    [SerializeField] private GameObject endHallway;
    [SerializeField] private Image newNotepadImg;
    [SerializeField] private UpdateUI notepadUI;

    public static event Action EndStartedEvent;
    public static event Action<string> EndDialogueEvent;

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
        // this will change the tasklist UI to have a face
        EndStartedEvent?.Invoke();
        // remove the front door (will be replaced by the door in the end hallway sequence
        //morningFrontDoor.gameObject.SetActive(false);
        // load in the end hallway additively
        //SceneSwitcher.LoadSceneOnTop("EndSequence");

        // unlock front door, reveal hallway
        Debug.Log("Front door unlocked");
        morningFrontDoor.locked = false;
        endHallway.SetActive(true);

        // show custom UI
        notepadUI.EnableEndGameNotepad(newNotepadImg);

        // Add dialogue
        EndDialogueEvent?.Invoke("Someone's at the door.");
        // play some noises, add stains on ground leading to front door?

    }

}
