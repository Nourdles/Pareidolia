using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private bool canInteract;

    private void Start() 
    {
        canInteract = false;
    }


    public void setObjectAsInteractable()
    {
        canInteract = true;
    }

    public void setObjectAsUninteractable()
    {
        canInteract = false;
    }

    public bool checkIfInteractable()
    {
        return canInteract;
    }
}
