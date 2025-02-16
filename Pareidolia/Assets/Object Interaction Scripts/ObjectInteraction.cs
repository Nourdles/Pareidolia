using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    protected InteractionManager interactionManager;
    
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

    protected abstract void InvokeInteractionEvent();
}
