using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Script for opening and closing notes
/// </summary>
public class OpenCloseNote : MonoBehaviour
{   
    InputAction tasklistAction;
    Renderer tasklist;
    [SerializeField] private GameObject tasklistcanvas;

    private bool noteOpen = false;
    private bool notePickedUp = false;

    private void Start() 
    {
        tasklistAction = InputSystem.actions.FindAction("Tasklist");
        tasklist = gameObject.GetComponent<Renderer>();
    }

    private void PickUpNotepad()
    {
        notePickedUp = true;
    }

    private void OpenNote()
    {
        Debug.Log("Opening note");
        // stop player from moving while reading

        noteOpen = true;
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
    
}
