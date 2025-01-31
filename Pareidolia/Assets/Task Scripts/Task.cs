using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected bool complete;
    
    void Start()
    {
        complete = false;
    }
    
}
