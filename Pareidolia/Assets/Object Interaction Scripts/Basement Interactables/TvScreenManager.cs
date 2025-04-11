using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TvScreenManager : MonoBehaviour
{
    [SerializeField]
    private EventReference fmodEvent;

    private EventInstance eventInstance;
    private bool isPlaying = false;

    void Start()
    {
        if (fmodEvent.IsNull) return;

        eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform, GetComponent<Rigidbody>());
        eventInstance.start();
        isPlaying = true;
    }

    void OnDestroy()
    {
        StopSoundImmediate();
    }

    public void StopSound(bool fadeOut = true)
    {
        if (eventInstance.isValid() && isPlaying)
        {
            eventInstance.stop(fadeOut ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
            isPlaying = false;
        }
    }

    private void StopSoundImmediate()
    {
        if (eventInstance.isValid())
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}
