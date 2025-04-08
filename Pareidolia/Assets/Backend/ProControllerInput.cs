// Wrapper script to check if the currently active device is a Switch Pro Controller
using UnityEngine;
using UnityEngine.InputSystem;

public static class ProControllerInput
{
    public static bool IsSwitchProConnected()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device.description.manufacturer.ToLower().Contains("nintendo") &&
                device.description.product.ToLower().Contains("wireless gamepad"))
            {
                return true;
            }
        }
        return false;
    }
}