using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    protected InteractionManager interactionManager;
    protected InventoryManager inventoryManager;
    public static event Action<string> DialoguePromptEvent;
    
    protected virtual void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        interactKey = InputSystem.actions.FindAction("Interact");
        inventoryManager = GameObject.FindWithTag("Inventory").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CanInteract())
        {
            InvokeInteractionEvent();
        }
    }

    protected virtual bool CanInteract()
    {
        return interactionManager.checkIfInteractable() && interactKey.WasPressedThisFrame();
    }

    protected void InvokeDialoguePromptEvent(string msg)
    {
        DialoguePromptEvent?.Invoke(msg);
    }

    // use this to do object unique interactions
    protected abstract void InvokeInteractionEvent();
}
