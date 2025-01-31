using System;
using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    private InteractionManager interactionManager;
    public static event Action InteractWithNote;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionManager.checkIfInteractable())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Invoking interaction event");
                InteractWithNote?.Invoke();
            }
        }
    }
}
