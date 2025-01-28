using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class CubeController : MonoBehaviour
{
    public float speed = 5f;

    // FMOD Audio Event Instance
    private EventInstance playerFootsteps;
    private bool isMoving = false;

    void Start()
    {
        // Initialize the footstep sound event instance
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;

        transform.Translate(move, Space.World);

        UpdateSound(moveX, moveZ);
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