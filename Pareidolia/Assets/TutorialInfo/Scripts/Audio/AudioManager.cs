using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private EventInstance ambienceEventInstance;

    [SerializeField] private EventReference ambienceEventReference;
    [SerializeField] private EventReference ambienceDreamReference;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate AudioManager found and destroyed.");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeAmbience(ambienceDreamReference);
    }

    private void InitializeAmbience(EventReference ambienceRef)
    {
        StopAmbience();

        ambienceEventInstance = CreateEventInstance(ambienceRef);
        ambienceEventInstance.start();
    }

    public void StartRoomAmbience()
    {
        InitializeAmbience(ambienceEventReference);
    }

    public void StartDreamAmbience()
    {
        InitializeAmbience(ambienceDreamReference);
    }

    public void StopAmbience()
    {
        if (ambienceEventInstance.isValid())
        {
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambienceEventInstance.release();
            ambienceEventInstance.clearHandle();
        }
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        return RuntimeManager.CreateInstance(eventReference);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void UpdateTaskLevel(float level)
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