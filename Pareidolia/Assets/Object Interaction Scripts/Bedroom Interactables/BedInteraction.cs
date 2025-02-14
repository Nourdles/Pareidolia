using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BedInteraction: MonoBehaviour
{
    private InputAction interactKey;
    private InteractionManager interactionManager;
    public static event Action BedInteractionEvent;

    private void Start() 
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        interactKey = InputSystem.actions.FindAction("Interact");
    }
    
    private void Update() {
        if (interactionManager.checkIfInteractable())
        {
            if (interactKey.WasPressedThisFrame())
            {
                Debug.Log("Invoking interaction event");
                BedInteractionEvent?.Invoke();
            }
        }
    }
}
