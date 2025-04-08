using System.Collections;
using UnityEngine;

/// <summary>
/// Script for moving the camera direction
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public Transform orientation; // orientation is an object that keeps track of the player's orientation
    public float mouseSens = 100f;
    float cameraVerticalRotation;
    float cameraHorizontalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // wait one frame to allow other scripts/objects to initialize
        yield return null;

        // get the local starting rotation
        Vector3 localStartRotation = transform.localEulerAngles;
        cameraVerticalRotation = localStartRotation.x;
        cameraHorizontalRotation = localStartRotation.y;

        // apply it manually to avoid startup snap
        transform.localRotation = Quaternion.Euler(cameraVerticalRotation, cameraHorizontalRotation, 0f);
    }


    // Update is called once per frame
    void Update()
    {

        float mouseX = Time.deltaTime * mouseSens * (
        ProControllerInput.IsSwitchProConnected() ? 
        Input.GetAxis("Switch Mouse X") : 
        Input.GetAxis("Mouse X")
        );
        float mouseY = Time.deltaTime * mouseSens * (
            ProControllerInput.IsSwitchProConnected() ? 
            Input.GetAxis("Switch Mouse Y") : 
            Input.GetAxis("Mouse Y")
        );

        cameraHorizontalRotation += mouseX;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        

        // move camera
        transform.rotation = Quaternion.Euler(cameraVerticalRotation, cameraHorizontalRotation, 0);

        // rotate the player object to face the new camera direction
        orientation.rotation = Quaternion.Euler(0, cameraHorizontalRotation, 0);

    }
}
