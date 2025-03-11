using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceChecker : MonoBehaviour
{
    [SerializeField] bool isKeyboardAndMouse;
    public static event Action<bool> UsingKBMEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.onActionChange += InputActionChangeCallback;
        isKeyboardAndMouse = true;
    }

    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction receivedInputAction = (InputAction) obj;
            InputDevice lastDevice = receivedInputAction.activeControl.device;

            isKeyboardAndMouse = lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse");
            UsingKBMEvent?.Invoke(isKeyboardAndMouse);
        }
    }

    void Update()
    {
        //Debug.Log("Keyboard and mouse input: " +  isKeyboardAndMouse);
    }
}
