using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    public static event Action<string> DialoguePromptEvent;
    protected String interactText = "";
    
    protected virtual void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
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
}
