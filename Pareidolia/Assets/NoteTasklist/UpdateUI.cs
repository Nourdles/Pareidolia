using UnityEngine;
using TMPro;

public class UpdateUI: MonoBehaviour
{
    [SerializeField] private TMP_Text[] notepadTextFields; // size 7
    [SerializeField] private string[] notepadText; // size 7
    // have a "Don't Look at the faces" images visible?
    

    private void Start() 
    {
        // set up original text
        notepadText[0] = "Morning To-Do List";
        notepadText[1] = "Make the bed";
        for (int i = 2; i<notepadText.Length; i++)
        {
            notepadText[i] = "";
        }
        updateTasks();
    }
    
   private void OnEnable() 
    {
        Task.CrossOutTaskEvent += completeTask;
        GameStateManager.LevelChangeEvent += changeTasks;
    }

    private void OnDisable() 
    {
       Task.CrossOutTaskEvent -= completeTask;
       GameStateManager.LevelChangeEvent -= changeTasks;
    }

    private void completeTask(int taskNum)
    {
        TMP_Text tasktocomplete = notepadTextFields[taskNum];
        tasktocomplete.fontStyle = FontStyles.Strikethrough;
    }

    // triggered by changelevelevent
    public void changeTasks(Levels lvl)
    {
        if (lvl == Levels.Morning) // morning lvl
        {
            notepadText[2] = "Make breakfast and coffee";
            //notepadText[3] = "Put the laundry in the wash";
            notepadText[3] = "Take a shower";
        } else if (lvl == Levels.Afternoon) // afternoon lvl
        {
            notepadText[0] = "Afternoon To-Do List";
            notepadText[1] = "Pick the trash up off the floors";
            //notepadText[2] = "Put the laundry in the dryer";
            //notepadText[3] = "Cook instant ramen for dinner";
            //notepadText[4] = "Wash the dishes";
            notepadText[2] = "Watch the newest episode of Octopus Competition";

        } else if (lvl == Levels.Evening) // evening lvl
        {
            notepadText[0] = "Night To-Do List";
            //notepadText[1] = "Feed the fish";
            //notepadText[2] = "Put away the laundry";
            //notepadText[3] = "Get a drink";
            //notepadText[4] = "Wipe the walls";
            notepadText[1] = "Board up the windows";
            notepadText[2] = "GO TO BED";
        }
        updateTasks();
    }

    private void updateTasks()
    {
        for (int txtfield = 0; txtfield < notepadTextFields.Length; txtfield++)
        {
            notepadTextFields[txtfield].text = notepadText[txtfield];
        }
    }
}
