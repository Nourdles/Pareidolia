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
            showerEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            if (interactKey.WasPressedThisFrame())
            {
                ShowerOnEvent?.Invoke();

                if (showerEventInstance.isValid())
                {
                    if (_showerStarted)
                    {
                        showerEventInstance.setPaused(false);
                    } else
                    {
                    Debug.Log("Starting shower");
                    showerEventInstance.start();
                    _showerStarted = true;
                    }
                }
            } 
            else if (interactKey.WasReleasedThisFrame())
            {
                ShowerOffEvent?.Invoke();

                if (showerEventInstance.isValid())
                {
                    showerEventInstance.setPaused(true);
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
        TubInteraction.GetIntoTubEvent += EnableScript;
        ShowerTask.ShowerComplete += ReleaseSFXInstance;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= EnableScript;
        ShowerTask.ShowerComplete -= ReleaseSFXInstance;
    }
}
