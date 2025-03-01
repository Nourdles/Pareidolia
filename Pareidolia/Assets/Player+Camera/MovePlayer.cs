//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    // FMOD Audio Event Instance
    private EventInstance playerFootsteps;
    private bool isMoving = false;

    void Start()
    {
        // Initialize the footstep sound event instance
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalinput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalinput;

        // snap the player to the surface it is currently on (prevents player from accelerating off stair steps/slopes)
        if (characterController.isGrounded) {
            moveDirection.y = -characterController.stepOffset / Time.deltaTime;
        }

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        UpdateSound(horizontalinput, verticalInput);
    }

    private void UpdateSound(float moveX, float moveZ)
    {
        bool shouldMove = moveX != 0 || moveZ != 0;

        playerFootsteps.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        if (shouldMove && !isMoving)
        {
            // Start playing footstep sound if not already playing
            playerFootsteps.start();
            isMoving = true;
        }
        else if (!shouldMove && isMoving)
        {
            // Stop the sound if movement stops
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }
    }

    private void OnDestroy()
    {
        // Release FMOD instance when object is destroyed
        playerFootsteps.release();
    }
}