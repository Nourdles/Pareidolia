using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialManager : MonoBehaviour
{
    //Message to display on UI
    public TextMeshProUGUI message;
    [SerializeField] GameObject textbox;
    public static event Action<string> TutorialDialogueEvent;
    [SerializeField] private GameObject player;


    public string InteractHotkey = "[E]";
    public string TasklistHotkey = "[Tab]";
    public enum TutorialState : ushort
    {
        INITIAL_STATE = 0,
        INTERACT_HOTKEY,
        NOTEPAD_HOTKEY,
        TASK_COMPLETION,
        REOPEN_NOTEPAD,
        COMPLETED 

    }
    private TutorialState state = TutorialState.INITIAL_STATE;

    public OpenCloseNote OpenCloseNote = null;
    public MakeBedTask MakeBedTask = null;

    //Config durations.
    private int SEC_TO_CALLS = 50;
    private int MESSAGE_DURATION_SEC = 20;

    private int INITIAL_MESSAGE_DELAY_SEC = 5;

    private int messageTimer = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = TutorialState.INITIAL_STATE;
        TutorialDialogueEvent?.Invoke("I have a lot to do today. I should grab my to-do list from my dresser");

        //To check if note is picked up and opened
        Debug.Assert(OpenCloseNote != null);

        Debug.Assert(player != null);

        //To monitor task completion status
        Debug.Assert(MakeBedTask != null);


    }

    // This update function is responsible for any logic checks that are due for the tutorial
    // (i.e checking if the player can currently interact with the tasklist
    void FixedUpdate()
    {
        //Disable script on completion.


        messageTimer -= 1;
        //Fade message if duration reacehd
        if (messageTimer == 0 && 
            new [] {
                TutorialState.NOTEPAD_HOTKEY,
                TutorialState.INTERACT_HOTKEY,
                TutorialState.COMPLETED}.Contains(state))
        {
            this.message.text = "";
            textbox.SetActive(false);
            if (state == TutorialState.COMPLETED)
            {
                this.enabled = false;
                return;
            }
        }

        //Non timer based events:
        switch (state)
        {
            case TutorialState.INITIAL_STATE:
                //Check if player is looking at an interactable object -> Next State
                if(player.GetComponent<PlayerInteract>().GetObjectInView() != null)
                {
                    NextState();
                }
                return;
            case TutorialState.INTERACT_HOTKEY:
                //Check if task list has been equipped -> Next State
                if (OpenCloseNote.isNotePickedUp())
                {
                    NextState();
                }
                return;
            case TutorialState.NOTEPAD_HOTKEY:
                //Check if notepad open -> Next State
                if (OpenCloseNote.isNoteOpen())
                {
                    NextState();
                }
                return;
            case TutorialState.TASK_COMPLETION:
                //Wait for task to be completed
                if (MakeBedTask.isCompleted())
                {
                    NextState();
                }
                return;
            case TutorialState.REOPEN_NOTEPAD:
                if (OpenCloseNote.isNoteOpen())
                {
                    NextState();
                }
                return;
        }

    }

    //This function is called to go to the next state
    //It is responsible for any logic that takes place in the transition
    public void NextState()
    {
        if (state == TutorialState.COMPLETED) {
            return;
        }
        state++;
        messageTimer = MESSAGE_DURATION_SEC * SEC_TO_CALLS;
        switch (state)
        {
            case TutorialState.INTERACT_HOTKEY:
                this.message.text = "Press " + InteractHotkey + " to interact with litup objects";
                textbox.SetActive(true);
                return;
            case TutorialState.NOTEPAD_HOTKEY:
                this.message.text = "Press " + TasklistHotkey + " to open/close your task list";
                textbox.SetActive(true);
                return;
            case TutorialState.TASK_COMPLETION:
                this.message.text = "Complete the listed task(s). Remember, you can press " + InteractHotkey + " to interact with objects";
                textbox.SetActive(true);
                return;
            case TutorialState.REOPEN_NOTEPAD:
                this.message.text = "Reopen your task list using " + TasklistHotkey + ". Tasks will be automatically crossed out.";
                textbox.SetActive(true);
                return;
            case TutorialState.COMPLETED:
                this.message.text = "Tutorial complete. Leave the bedroom when you're ready to start your day.";
                textbox.SetActive(true);
                return;
            default:
                return;
        }
    }


    public TutorialState getTutorialState()
    {
        return state;
    }
}
