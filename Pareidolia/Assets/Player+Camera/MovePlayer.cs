using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Script for moving the player with surface-specific footstep sounds
/// </summary>
public class MovePlayer : MonoBehaviour
{
    public CharacterController characterController;
    public Transform orientation;
    public float moveSpeed = 1;
    public float footstepInterval = 0.4f; // Adjust footstep frequency

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    
    private EventInstance playerFootsteps;
    private bool isMoving = false;
    private int surfaceType = 0; // Default to Wood
    private float footstepTimer = 0f;
    
    void Start()
    {
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
        playerFootsteps.setParameterByName("Surface", surfaceType);
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (characterController.isGrounded)
        {
            moveDirection.y = -characterController.stepOffset / Time.deltaTime;
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        UpdateSound(horizontalInput, verticalInput);
    }

    private void UpdateSound(float moveX, float moveZ)
    {
        bool shouldMove = moveX != 0 || moveZ != 0;

        playerFootsteps.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        playerFootsteps.setParameterByName("Surface", surfaceType);

        if (shouldMove)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                playerFootsteps.start();
                footstepTimer = 0f;
            }
            isMoving = true;
        }
        else if (!shouldMove && isMoving)
        {
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            isMoving = false;
            footstepTimer = 0f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.collider.tag)
        {
            case "Wood":
                surfaceType = 0;
                break;
            case "Tile":
                surfaceType = 1;
                break;
            case "Carpet":
                surfaceType = 2;
                break;
            case "Stairs":
                surfaceType = 3;
                break;
            default:
                surfaceType = 0; // Default to Wood
                break;
        }

        playerFootsteps.setParameterByName("Surface", surfaceType);
    }

    private void OnDestroy()
    {
        playerFootsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        playerFootsteps.release();
    }
}
