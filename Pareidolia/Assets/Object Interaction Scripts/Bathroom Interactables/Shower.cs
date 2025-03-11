using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Shower : MonoBehaviour
{
    [SerializeField] private bool _inShower;
    private String inputMasking;
    private bool _showInstructions = false;
    private bool _showerStarted = false;
    private InputAction interactKey;
    private FMOD.Studio.EventInstance showerEventInstance;
    public static event Action ShowerOnEvent;
    public static event Action ShowerOffEvent;
    public static event Action<String> ShowerInstructions;
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
            if (_showInstructions)
            {
               ShowerInstructions?.Invoke("Hold <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to shower"); 
            }
            
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
        _showInstructions = false;
    }

    private void EnableScript()
    {
        _inShower = true;
        _showInstructions = true;
    }

    private void UpdateControllerScheme(bool usingKBM)
    {
        if (usingKBM)
        {
            inputMasking = "Keyboard&Mouse";
        } else
        {
            inputMasking = "Gamepad";
        }
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += EnableScript;
        ShowerTask.ShowerComplete += ReleaseSFXInstance;
        InputDeviceChecker.UsingKBMEvent += UpdateControllerScheme;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= EnableScript;
        ShowerTask.ShowerComplete -= ReleaseSFXInstance;
        InputDeviceChecker.UsingKBMEvent -= UpdateControllerScheme;
    }
}
