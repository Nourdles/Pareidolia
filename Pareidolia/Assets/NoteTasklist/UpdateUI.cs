using UnityEngine;
using TMPro;

public class UpdateUI: MonoBehaviour
{
    [SerializeField] private TMP_Text noteTextAreaUI;

    private void UIMarkAsComplete()
    {
        noteTextAreaUI.fontStyle = FontStyles.Strikethrough;
    }

    private void OnEnable() 
    {
        MakeBedTask.CompleteMakeBedTask += UIMarkAsComplete;
    }

    private void OnDisable() 
    {
        MakeBedTask.CompleteMakeBedTask -= UIMarkAsComplete;
    }
}
