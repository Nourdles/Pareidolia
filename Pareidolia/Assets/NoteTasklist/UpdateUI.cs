using UnityEngine;
using TMPro;
using System;
using FMODUnity;
using UnityEngine.UIElements;

public class UpdateUI: MonoBehaviour
{
    [SerializeField] private TMP_Text[] notepadTextFields; // size 7
    [SerializeField] private string[] notepadText; // size 7
    [SerializeField] private string tasklistUpdateSFXPath = "event:/SFX/Tasklist Update";
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

    private void OnEnable() 
    {
        Task.CrossOutTaskEvent += completeTask;
        TaskManager.MoveToNextTask += changeTextColor;
        //GameStateManager.LevelChangeEvent += changeTasks;
    }

    private void OnDisable() 
    {
       Task.CrossOutTaskEvent -= completeTask;
       TaskManager.MoveToNextTask -= changeTextColor;
       //GameStateManager.LevelChangeEvent -= changeTasks;
    }

    private void changeTextColor(int taskNum)
    {
        TMP_Text tasktocomplete = notepadTextFields[taskNum];
        tasktocomplete.color = ActiveColor;
    }

    private void completeTask(int taskNum)
    {
        TMP_Text tasktocomplete = notepadTextFields[taskNum];
        tasktocomplete.fontStyle = FontStyles.Strikethrough;
        tasktocomplete.color = InactiveColor;
    }

    private void updateTasks()
    {
        for (int txtfield = 0; txtfield < notepadTextFields.Length; txtfield++)
        {
            notepadTextFields[txtfield].text = notepadText[txtfield];
            if (txtfield == 0 || txtfield == 1)
            {
                notepadTextFields[txtfield].color = ActiveColor;
            } else 
            {
                Debug.Log("Setting textfield "+ txtfield + " to inactive color");
                notepadTextFields[txtfield].color = InactiveColor;
            }
            
        }
        TasksUpdatedEvent?.Invoke();
    }
}
