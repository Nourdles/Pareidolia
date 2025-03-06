using TMPro;
using UnityEngine;

public class DialogueUIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueField;
    int MSG_TIME = 5;
    float timetodisappear;

    void Update()
    {
        if (dialogueField.enabled && Time.time >= timetodisappear)
        {
            dialogueField.enabled = false;
        }
    }

    void OnEnable()
    {
        ObjectInteraction.DialoguePromptEvent += UpdateDialogueText;
        TutorialManager.TutorialDialogueEvent += UpdateDialogueText;
        BasementDoorScriptedEvent.BasementDoorDialogueEvent += UpdateDialogueText;
    }

    void OnDisable()
    {
        ObjectInteraction.DialoguePromptEvent -= UpdateDialogueText;
        TutorialManager.TutorialDialogueEvent -= UpdateDialogueText;
        BasementDoorScriptedEvent.BasementDoorDialogueEvent -= UpdateDialogueText;
    }

    private void UpdateDialogueText(string msg)
    {
        dialogueField.text = msg;
        EnableText();
    }

    private void EnableText()
    {
        dialogueField.enabled = true;
        timetodisappear = Time.time + MSG_TIME;
    }
}
