using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    protected InteractionManager interactionManager;
    public static event Action<string> DialoguePromptEvent;
    
    protected virtual void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        interactKey = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CanInteract())
        {
            InvokeInteractionEvent();
        }
    }

    protected bool CanInteract()
    {
        return interactionManager.checkIfInteractable() && interactKey.WasPressedThisFrame();
    }

    protected void InvokeDialoguePromptEvent(string msg)
    {
        DialoguePromptEvent?.Invoke(msg);
    }

    protected abstract void InvokeInteractionEvent();
}
