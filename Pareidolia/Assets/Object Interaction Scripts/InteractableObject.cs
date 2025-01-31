using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    private bool canInteract;

    private void Start() 
    {
        canInteract = false;
    }


    protected abstract void interact();

    public void setObjectAsInteractable()
    {
        canInteract = true;
    }

    public void setObjectAsUninteractable()
    {
        canInteract = false;
    }

}
