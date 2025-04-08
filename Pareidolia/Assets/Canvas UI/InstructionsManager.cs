using TMPro;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _instructionsField;
    private void DisplayInteractInstructions(string msg)
    {
        _instructionsField.text = msg;
    }

    private void ResetInstructions()
    {
        _instructionsField.text = "";
    }

    void OnEnable()
    {
        ShowerInstructionsController.ShowerInstructionsEvent += DisplayInteractInstructions;
        ShowerTask.ShowerComplete += ResetInstructions;
    }

    void OnDisable()
    {
        ShowerInstructionsController.ShowerInstructionsEvent -= DisplayInteractInstructions;
        ShowerTask.ShowerComplete -= ResetInstructions;
    }
}
