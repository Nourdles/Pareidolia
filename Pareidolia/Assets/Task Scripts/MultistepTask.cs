using UnityEngine;

public class MultistepTask : Task
{
    // reference to subtasks
    [SerializeField] protected int numTasksRequired;
    [SerializeField] protected int numTasksCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        numTasksCompleted = 0;
    }

    // increment completed tasks by 1 and check if completed all subtasks
    protected void completeSubTask()
    {
        numTasksCompleted += 1;
        if (numTasksCompleted == numTasksRequired)
        {
            completeTask();
        }
    }
}
