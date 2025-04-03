using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectInteraction : MonoBehaviour
{
    protected InputAction interactKey;
    protected Task task = null;
    protected TaskManager taskManager;
    public static event Action<string> DialoguePromptEvent;
    [SerializeField] protected String interactText = "";
    [SerializeField] protected String inputMasking = "Keyboard&Mouse";
    
    protected virtual void Start()
    {
        InputDeviceChecker.UsingKBMEvent += SetDeviceController;
        interactKey = InputSystem.actions.FindAction("Interact");
        inputMasking = "Keyboard&Mouse";
        taskManager = GameObject.FindWithTag("TaskManager").GetComponent<TaskManager>();
        Debug.Log("Taskmanager value in object interaction scripts: " + taskManager);

    }

    public void interact(GameObject objectInHand)
    {
        if (CheckIfTaskActive())
        {
            interactaction(objectInHand);
        } else
        {
            InvokeDialoguePromptEvent("I should " + taskManager.GetCurrentTask().ToString() + " first");
        }
    }

    protected abstract void interactaction(GameObject objectInHand);

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

    protected bool CheckIfTaskActive()
    {
        if (task != null)
        {
            return task.GetActiveStatus();
        } else
        {
            return true;
            // will always return true for objects unassociated with a task
        }
    }
}
