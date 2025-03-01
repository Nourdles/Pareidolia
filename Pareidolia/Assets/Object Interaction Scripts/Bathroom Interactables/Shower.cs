using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Shower : MonoBehaviour
{
    [SerializeField] private bool _inShower;
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
            if (interactKey.WasPressedThisFrame())
            {
                Debug.Log("Showering");
                ShowerOnEvent?.Invoke();

                if (showerEventInstance.isValid())
                {
                    showerEventInstance.start();
                }
            } 
            else if (interactKey.WasReleasedThisFrame())
            {
                ShowerOffEvent?.Invoke();

                if (showerEventInstance.isValid())
                {
                    showerEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
            }
        }
    }

    private void EnableScript()
    {
        _inShower = true;
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += EnableScript;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= EnableScript;
    }
}
