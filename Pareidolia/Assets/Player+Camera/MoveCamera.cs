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
    void Start()
    {
        // prevent cursor from moving off the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens;

        cameraHorizontalRotation += mouseX;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        // move camera
        transform.rotation = Quaternion.Euler(cameraVerticalRotation, cameraHorizontalRotation, 0);

        // rotate the player obejct to face the new camera direction
        orientation.rotation = Quaternion.Euler(0, cameraHorizontalRotation, 0);

    }
}
