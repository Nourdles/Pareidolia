using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{

    private EventInstance ambienceEventInstance;

    [SerializeField] private EventReference ambienceEventReference;
    
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

    }

    private void Start()
    {
        InitializeAmbience(ambienceEventReference);
    }

    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        // Stop the previous ambience if it's running
        if (ambienceEventInstance.isValid())
        {
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void UpdateTaskLevel(int level)
    {
        if (ambienceEventInstance.isValid())
        {
            ambienceEventInstance.setParameterByName("Task Level", level);
        }
        else
        {
            Debug.LogWarning("Ambience event instance is not valid when trying to set Task Level.");
        }
    }

}