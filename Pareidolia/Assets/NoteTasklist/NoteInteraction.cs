using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteInteraction : MonoBehaviour
{
    InputAction keyInteraction;
    private InteractionManager interactionManager;
    public static event Action NotepadPickedUp;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
        keyInteraction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionManager.checkIfInteractable())
        {
            if (keyInteraction.WasPressedThisFrame())
            {
                Debug.Log("Invoking interaction event");
                NotepadPickedUp?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
