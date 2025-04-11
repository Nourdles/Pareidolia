using UnityEngine;
using TMPro;
using System;
using FMODUnity;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class UpdateUI: MonoBehaviour
{
    [SerializeField] private TMP_Text[] notepadTextFields; // size 6
    [SerializeField] private string[] notepadText; // size 6
    [SerializeField] private GameObject[] taskMarkers; //size 5
    [SerializeField] private string tasklistUpdateSFXPath = "event:/SFX/Tasklist Update";
    [SerializeField] private TMP_Text laundryProg;
    private static Color InactiveColor = new Color(.349f, .274f, .211f);
    private static Color ActiveColor = new Color(0f, 0f, 0f);
    public static event Action TasksUpdatedEvent;

    private void updateTaskListSFX()
    {
        RuntimeManager.PlayOneShot(tasklistUpdateSFXPath, transform.position);
    }

    private void Start()
    {
        // determine which level/scene it currently is, and display the associated tasks
        if (GameStateManager.levelState == Levels.Tutorial)
        {
            notepadText[0] = "Morning To-Do List";
            notepadText[1] = "Make the bed";
            for (int i = 2; i < notepadText.Length; i++)
            {
                notepadText[i] = "";
            }
        }
        else if (GameStateManager.levelState == Levels.Morning)
        {
            // Update task list sfx
            updateTaskListSFX();
            notepadText[0] = "Morning To-Do List";
            notepadText[1] = "Make coffee";
            notepadText[2] = "Make cereal";
            notepadText[3] = "Watch TV";
            notepadText[4] = "Take a shower";
            notepadText[5] = "Put dirty clothes in the wash";
        }
        updateTasks();
    }

    private void UpdateClothingCount(string progress)
    {
        Debug.Log("Updating task UI");
        if (progress == "")
        {
            laundryProg.text = "";
        } else
        {
            laundryProg.text = "(Dirty clothes picked up: " + progress + ")";
        }
    }
     private void ActivateClothingCount()
     {
        Debug.Log("Activating clothing prog");
        laundryProg.text = "(Dirty clothes picked up: 0/4)";
     }

    private void OnEnable() 
    {
        Task.CrossOutTaskEvent += completeTask;
        TaskManager.MoveToNextTask += moveTaskMarker;
        LaundryBinInteraction.UpdateClothingCount += UpdateClothingCount;
        ShowerTask.ShowerComplete += ActivateClothingCount;
    }

    private void OnDisable() 
    {
       Task.CrossOutTaskEvent -= completeTask;
       TaskManager.MoveToNextTask -= moveTaskMarker;
       LaundryBinInteraction.UpdateClothingCount -= UpdateClothingCount;
       ShowerTask.ShowerComplete -= ActivateClothingCount;
    }

    private void moveTaskMarker(int taskNum)
    {
        //TMP_Text tasktocomplete = notepadTextFields[taskNum];
        //tasktocomplete.color = ActiveColor;

        taskMarkers[taskNum-1].SetActive(true);
    }

    private void completeTask(int taskNum)
    {
        TMP_Text tasktocomplete = notepadTextFields[taskNum];
        tasktocomplete.fontStyle = FontStyles.Strikethrough;
        //tasktocomplete.color = InactiveColor;

        taskMarkers[taskNum-1].SetActive(false);
    }

    private void updateTasks()
    {
        for (int txtfield = 0; txtfield < notepadTextFields.Length; txtfield++)
        {
            notepadTextFields[txtfield].text = notepadText[txtfield];
            /*
            if (txtfield == 0 || txtfield == 1)
            {
                notepadTextFields[txtfield].color = ActiveColor;
            } else 
            {
                Debug.Log("Setting textfield "+ txtfield + " to inactive color");
                notepadTextFields[txtfield].color = InactiveColor;
            }
            */
            
        }
        TasksUpdatedEvent?.Invoke();
    }

    /// <summary>
    /// called by StartEndSequenceEvent at the end of game to change the notepad ui to be spookier
    /// </summary>
    public void EnableEndGameNotepad(UnityEngine.UI.Image newNotepadImg)
    {
        // change header
        notepadTextFields[0].text = "Someone is outside.";

        // clear other text fields
        for (int txtfield = 1; txtfield < notepadTextFields.Length; txtfield++)
        {
            notepadTextFields[txtfield].text = "";
        }
        updateTaskListSFX();

        // change to image
        Color color = newNotepadImg.color;
        color.a = 1f;
        newNotepadImg.color = color;
        TasksUpdatedEvent?.Invoke();

    }
}
