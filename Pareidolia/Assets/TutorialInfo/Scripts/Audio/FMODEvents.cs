using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Dream")]
    [field: SerializeField] public EventReference Dream { get; private set; }
    
    [field: Header("Ambience1")]
    [field: SerializeField] public EventReference ambience1 { get; private set; }

    [field: Header("Ambience2")]
    [field: SerializeField] public EventReference ambience2 { get; private set; }
    
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}