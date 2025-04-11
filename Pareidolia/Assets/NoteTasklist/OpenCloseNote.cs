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
    public bool isPaused = false;


    public static event Action NotepadFirstCheckEvent;
    public static event Action NoteOpenedEvent;

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
            NoteOpenedEvent?.Invoke();
            _firstOpen = false;
        }
        else
        {
            NoteOpenedEvent?.Invoke();
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
        if (tasklist != null)
        {
            tasklist.enabled = noteOpen;
        }

        if (tasklistcanvas != null)
        {
            tasklistcanvas.SetActive(noteOpen);
        }

        if (notePickedUp)
        {
            if (tasklistAction.WasPressedThisFrame() && !isPaused)
            {
                Debug.Log("Tasklist button was pressed");
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

    private void SetPaused(bool pause)
    {
        isPaused = pause;
    }

    private void OnEnable() 
    {
        NoteInteraction.NotepadPickedUp += PickUpNotepad;
        DeathManager.DeathSceneEvent += CloseNote;
        SofaInteraction.TVStartEvent += CloseNote;
        PauseManager.PauseGameEvent += SetPaused;
    }

    private void OnDisable() 
    {
        NoteInteraction.NotepadPickedUp -= PickUpNotepad;    
        DeathManager.DeathSceneEvent -= CloseNote;
        SofaInteraction.TVStartEvent -= CloseNote;
        PauseManager.PauseGameEvent -= SetPaused;
    }

    public bool isNotePickedUp()
    {
        return notePickedUp;
    }

    public bool isNoteOpen()
    { return noteOpen; }
    
}
