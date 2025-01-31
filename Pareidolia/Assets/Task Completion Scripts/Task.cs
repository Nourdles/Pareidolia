using UnityEngine;

public abstract class Task : MonoBehaviour
{
    private bool completeStatus;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        completeStatus = false;
    }

    protected virtual void completeTask()
    {
        completeStatus = true;
    }
}
