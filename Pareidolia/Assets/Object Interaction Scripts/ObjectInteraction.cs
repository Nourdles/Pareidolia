using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    public static event Action<string> DialoguePromptEvent;
    
    protected virtual void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
    }

    public abstract void interact(GameObject objectInHand);

    protected void InvokeDialoguePromptEvent(string msg)
    {
        DialoguePromptEvent?.Invoke(msg);
    }
}
