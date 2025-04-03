using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowerInstructionsController : MonoBehaviour
{
    private InputAction interactKey;
    private string inputMasking;
    private bool activated = false;
    public static event Action<string> ShowerInstructionsEvent;

    void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
    }

    private void TriggerShowerInstructions()
    {
        ShowerInstructionsEvent?.Invoke("Hold <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to shower");
        if (!activated)
        {
            activated = true;
        }
    }

    private void UpdateControllerScheme(bool usingKBM)
    {
        if (usingKBM)
        {
            inputMasking = "Keyboard&Mouse";
        } else
        {
            inputMasking = "Gamepad";
        }
        if (activated)
        {
            TriggerShowerInstructions();
        }
    }

    private void Deactivate()
    {
        Destroy(this);
    }

    void OnEnable()
    {
        InputDeviceChecker.UsingKBMEvent += UpdateControllerScheme;
        ShowerInteraction.GetIntoTubEvent += TriggerShowerInstructions;
        ShowerTask.ShowerComplete += Deactivate;
    }

    void OnDisable()
    {
        InputDeviceChecker.UsingKBMEvent -= UpdateControllerScheme;
        ShowerInteraction.GetIntoTubEvent -= TriggerShowerInstructions;
        ShowerTask.ShowerComplete -= Deactivate;
    }
}
