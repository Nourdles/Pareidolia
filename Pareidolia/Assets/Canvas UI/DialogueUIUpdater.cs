using TMPro;
using UnityEngine;

public class DialogueUIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueField;
    [SerializeField] private GameObject textbox;
    int MSG_TIME = 7;
    float timetodisappear;
    private bool _tutMSG = false;

    void Update()
    {
        if (_tutMSG)
        {
            // don't make tutorial msg disappear
        } else if (dialogueField.enabled && Time.time >= timetodisappear)
        {
            dialogueField.enabled = false;
            textbox.SetActive(false);
        }
    }

    void OnEnable()
    {
        ObjectInteraction.DialoguePromptEvent += UpdateDialogueText;
        TutorialManager.TutorialDialogueEvent += TutorialTextEnable;
        NoteInteraction.NotepadPickedUp += TutorialTextDisable;
        BasementDoorScriptedEvent.BasementDoorDialogueEvent += UpdateDialogueText;
    }

    void OnDisable()
    {
        ObjectInteraction.DialoguePromptEvent -= UpdateDialogueText;
        TutorialManager.TutorialDialogueEvent -= TutorialTextEnable;
        NoteInteraction.NotepadPickedUp -= TutorialTextDisable;
        BasementDoorScriptedEvent.BasementDoorDialogueEvent -= UpdateDialogueText;
    }

    private void UpdateDialogueText(string msg)
    {
        dialogueField.text = msg;
        EnableTempText();
    }

    private void TutorialTextEnable(string msg)
    {
        dialogueField.text = msg;
        _tutMSG = true;
        EnableTempText();
    }

    private void TutorialTextDisable()
    {
        dialogueField.enabled = false;
        textbox.SetActive(false);
        _tutMSG = false;
    }

    private void EnableTempText()
    {
        dialogueField.enabled = true;
        textbox.SetActive(true);
        timetodisappear = Time.time + MSG_TIME;
    }
}
