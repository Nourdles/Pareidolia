using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Script for moving the player
/// </summary>
public class MovePlayer : MonoBehaviour
{
    public CharacterController characterController;
    public Transform orientation;
    public float moveSpeed = 1;

    float horizontalinput;
    float verticalInput;

    Vector3 moveDirection;


    // Update is called once per frame
    void Update()
    {
        horizontalinput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalinput;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
