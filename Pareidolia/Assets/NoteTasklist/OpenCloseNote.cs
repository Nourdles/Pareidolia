using UnityEngine;
using TMPro;

/// <summary>
/// Script for opening and closing notes
/// </summary>
public class OpenCloseNote : MonoBehaviour
{   
    public KeyCode closeKey; // key to press to exit note
    public Rigidbody player;

    public GameObject noteCanvas;
    public TMP_Text noteTextAreaUI;
    public string text; // text to appear on the note

    private bool noteOpen = false;

    public void OpenNote()
    {
        Debug.Log("Opening note");
        noteTextAreaUI.text = text;
        noteCanvas.SetActive(true);

        // stop player from moving while reading
        DisablePlayer();

        noteOpen = true;
    }

    public void CloseNote()
    {
        noteCanvas.SetActive(false);

        // enable player movement
        EnablePlayer();

        noteOpen = false;
    }

    public void DisablePlayer()
    {
        player.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void EnablePlayer()
    {
        player.constraints = RigidbodyConstraints.None;
    }

    private void Update()
    {
        noteCanvas.SetActive(noteOpen);
        if (noteOpen)
        {
            if (Input.GetKeyDown(closeKey))
            {
                CloseNote();
            }
        }
    }

    private void OnEnable() 
    {
        NoteInteraction.InteractWithNote += OpenNote;    
    }

    private void OnDisable() 
    {
        NoteInteraction.InteractWithNote -= OpenNote;    
    }
    
}
