using System;
using UnityEngine;

public class BedInteraction: MonoBehaviour
{
    private InteractionManager interactionManager;
    public static event Action BedInteractionEvent;

    private void Start() 
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
    }
    
    private void Update() {
        if (interactionManager.checkIfInteractable())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Invoking interaction event");
                BedInteractionEvent?.Invoke();
            }
        }
    }
}
