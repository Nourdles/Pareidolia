using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shower : MonoBehaviour
{
    [SerializeField] private bool _inShower;
    private InputAction interactKey;
    public static event Action ShowerOnEvent;
    public static event Action ShowerOffEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactKey = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (_inShower)
        {
            if (interactKey.WasPressedThisFrame())
            {
                Debug.Log("Showering");
                ShowerOnEvent?.Invoke();
            } else if (interactKey.WasReleasedThisFrame())
            {
                ShowerOffEvent?.Invoke();
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
