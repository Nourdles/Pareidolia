using UnityEngine;
/// <summary>
/// Keep track of what tasks have been completed and respond accordingly (event, advance to nect elevel, etc)
/// 
/// </summary>
public class TutorialTaskTracker : MonoBehaviour
{
    int numTasksCompleted = 0;
    int numTasksGoal = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        BedInteraction.BedInteractionEvent += CountTasksCompleted;
        DoorInteraction.DoorFirstOpeningEvent += CountTasksCompleted;

    }

    private void OnDisable()
    {
        BedInteraction.BedInteractionEvent -= CountTasksCompleted;
        DoorInteraction.DoorFirstOpeningEvent -= CountTasksCompleted;
    }

    private void CountTasksCompleted()
    {
        numTasksCompleted++;
        Debug.Log("A task has been completed");
        if (numTasksCompleted == numTasksGoal)
        {
            GameStateManager.StartMorning();
        }
    }
}
