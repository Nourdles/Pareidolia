using UnityEngine;

// D
public class EndHallwayCloseDoor : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private DoorInteraction frontDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            if (frontDoor.CheckOpen())
            {
                // close the door
                frontDoor.interact(null);
            }
            // lock door, set locked text
            frontDoor.LockDoor();
            frontDoor.SetLockedDialogue("I can't go back.");
        }

        // disable trigger so it cant be triggered again
        gameObject.SetActive(false);
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }
}
