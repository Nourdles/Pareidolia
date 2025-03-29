using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using System;

/// <summary>
/// Script for opening and closing notes
/// </summary>
public class OpenCloseNote : MonoBehaviour
{   
    InputAction tasklistAction;
    Renderer tasklist;
    [SerializeField] private GameObject tasklistcanvas;
    private bool _firstOpen = true;
    private bool noteOpen = false;
    [SerializeField] private bool notePickedUp = false;
    // Tasklist SFX for opening
    [SerializeField] private string tasklistSFXPath = "event:/SFX/Tasklist";
    // Pickup SFX
    [SerializeField] private string notepadPickupSFX = "event:/SFX/NotepadPickup";


    public static event Action NotepadFirstCheckEvent;

    private void Start() 
    {
        tasklistAction = InputSystem.actions.FindAction("Tasklist");
        tasklist = gameObject.GetComponent<Renderer>();
    }

    private void PickUpNotepad()
    {
        notePickedUp = true;
        RuntimeManager.PlayOneShot(notepadPickupSFX, transform.position);
    }

    private void OpenNote()
    {
        Debug.Log("Opening note");
        // stop player from moving while reading

        // Play the FMOD sound here
        RuntimeManager.PlayOneShot(tasklistSFXPath, transform.position);
        
        noteOpen = true;
        if (_firstOpen)
        {
            NotepadFirstCheckEvent?.Invoke();
            _firstOpen = false;
        }
    }

    private void CloseNote()
    {
        Debug.Log("Closing note");
        // enable player movement

        noteOpen = false;
    }


    private void Update()
    {
        // change to check if open button if being pressed and that note has been picked up
        tasklist.enabled = noteOpen;
        tasklistcanvas.SetActive(noteOpen);
        if (notePickedUp)
        {
            if (tasklistAction.WasPressedThisFrame())
            {
                if (noteOpen) 
                {
                    CloseNote();
                }
                else 
                {
                    OpenNote();
                }
            }
        }
    }

    private void OnEnable() 
    {
        NoteInteraction.NotepadPickedUp += PickUpNotepad;    
    }

    private void OnDisable() 
    {
        NoteInteraction.NotepadPickedUp -= PickUpNotepad;    
    }

    public bool isNotePickedUp()
    {
        return notePickedUp;
    }

    public bool isNoteOpen()
    { return noteOpen; }
    
}
