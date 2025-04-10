using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DoorSoundOnStart : MonoBehaviour
{
    [SerializeField] private EventReference doorEvent;
    private EventInstance doorEventInstance;

    void Start()
    {
        doorEventInstance = RuntimeManager.CreateInstance(doorEvent);

        RuntimeManager.AttachInstanceToGameObject(doorEventInstance, transform, GetComponent<Rigidbody>());

        doorEventInstance.start();
    }

    void OnDestroy()
    {

        if (doorEventInstance.isValid())
        {
            doorEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            doorEventInstance.release();
        }
    }
}