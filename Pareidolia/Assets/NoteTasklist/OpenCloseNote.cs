using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Script for opening and closing notes
/// </summary>
public class OpenCloseNote : MonoBehaviour
{   
    InputAction tasklistAction;
    public Rigidbody player;
    public GameObject noteCanvas;
    private bool noteOpen = false;
    private bool notePickedUp = false;

    private void Start() 
    {
        tasklistAction = InputSystem.actions.FindAction("Tasklist");
    }

    private void PickUpNotepad()
    {
        notePickedUp = true;
    }

    private void OpenNote()
    {
        Debug.Log("Opening note");
        // noteTextAreaUI.text = text; move this to a separate UI manager

        // stop player from moving while reading
        DisablePlayer();

        noteOpen = true;
    }

    private void CloseNote()
    {
        Debug.Log("Closing note");
        // enable player movement
        EnablePlayer();

        noteOpen = false;
    }

    private void DisablePlayer()
    {
        player.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void EnablePlayer()
    {
        player.constraints = RigidbodyConstraints.None;
    }


    private void Update()
    {
        // change to check if open button if being pressed and that note has been picked up
        noteCanvas.SetActive(noteOpen);
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
    
}
