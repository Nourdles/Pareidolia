using UnityEngine;

/// <summary>
/// Move the camera to an object's position
/// </summary>
public class CameraPosition : MonoBehaviour
{
    /* Set this object to be PlayerOrientation in the player prefab 
     * to get the camera to move with the player */
    public Transform cameraPosition; 

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
