using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    public static event Action<string> DialoguePromptEvent;
    [SerializeField] protected String interactText = "";
    public static event Action InteractTextEvent;
    [SerializeField] protected String inputMasking = "Keyboard&Mouse";
    
    protected virtual void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
        inputMasking = "Keyboard&Mouse";
    }

    public abstract void interact(GameObject objectInHand);

    protected void InvokeDialoguePromptEvent(string msg)
    {
        DialoguePromptEvent?.Invoke(msg);
    }

    protected void SetInteractable()
    {
        gameObject.tag = "InteractableObject";
    }

    protected void SetUninteractable()
    {
        gameObject.tag = "Untagged";
    }

    protected void ResetInteractionText()
    {
        interactText = "";
    }

    public string GetInteractText()
    {
        return interactText;
    }

    protected abstract void UpdateInteractText();
    
    protected void SetDeviceController(bool usingKBM)
    {
        if (usingKBM)
        {
            inputMasking = "Keyboard&Mouse";
        } else
        {
            inputMasking = "Gamepad";
        }
        UpdateInteractText();
    }

    void OnEnable()
    {
        InputDeviceChecker.UsingKBMEvent += SetDeviceController;
    }

    void OnDisable()
    {
        InputDeviceChecker.UsingKBMEvent -= SetDeviceController;
    }
}
