using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Shower : MonoBehaviour
{
    [SerializeField] private bool _inShower;
    private bool _showerStarted = false;
    private InputAction interactKey;
    private FMOD.Studio.EventInstance showerEventInstance;
    public static event Action ShowerOnEvent;
    public static event Action ShowerOffEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");

        showerEventInstance = RuntimeManager.CreateInstance("event:/SFX/Shower");
    }

    void Update()
    {
        if (_inShower)
        {
            showerEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

            if (interactKey.WasPressedThisFrame())
            {
                ShowerOnEvent?.Invoke();

                if (!_showerStarted)
                {
                    Debug.Log("Starting shower");

                    showerEventInstance = RuntimeManager.CreateInstance("event:/SFX/Shower");
                    RuntimeManager.AttachInstanceToGameObject(showerEventInstance, transform, GetComponent<Rigidbody>());
                    showerEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

                    showerEventInstance.start();
                    _showerStarted = true;
                }
                else if (showerEventInstance.isValid())
                {
                    showerEventInstance.setPaused(false);
                }
            }
            else if (interactKey.WasReleasedThisFrame())
            {
                ShowerOffEvent?.Invoke();

                if (showerEventInstance.isValid())
                {
                    showerEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    _showerStarted = false;
                }
            }
        }
    }


    private void ReleaseSFXInstance()
    {
        showerEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        showerEventInstance.release();
    }

    private void EnableScript()
    {
        _inShower = true;
    }

    void OnEnable()
    {
        ShowerInteraction.GetIntoTubEvent += EnableScript;
        ShowerTask.ShowerComplete += ReleaseSFXInstance;
    }

    void OnDisable()
    {
        ShowerInteraction.GetIntoTubEvent -= EnableScript;
        ShowerTask.ShowerComplete -= ReleaseSFXInstance;
    }
}
