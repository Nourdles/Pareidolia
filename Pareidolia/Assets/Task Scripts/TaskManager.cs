using System;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Task currentTask;
    [SerializeField] private GameStateManager gameStateManager;
    public static event Action<int> MoveToNextTask;
    private Levels _currLvl;

    private int taskLevel = 0; // FMOD ambience layer tracking I've defined as "tasklevel"

    void Start()
    {
        gameStateManager = FindAnyObjectByType<GameStateManager>();
        _currLvl = gameStateManager.GetLevelState();

        if (_currLvl == Levels.Tutorial)
        {
            currentTask = gameObject.GetComponentInChildren<MakeBedTask>();
        } else
        {
            currentTask = gameObject.GetComponentInChildren<MakeCoffeeTask>();
        }
        currentTask.SetAsCurrent();

        // Initial FMOD parameter for Task Level
        FMODEvents.instance.UpdateTaskLevel(taskLevel); 
    }

    private void completeTask()
    {
        if (currentTask != null)
        {
            currentTask = currentTask.GetNextTask(); // go to next task
            if (currentTask != null)
            {
                taskLevel = currentTask.GetTasknum(); // change "task level" based on tasknum
                MoveToNextTask?.Invoke(currentTask.GetTasknum());
                currentTask.SetAsCurrent();

                UpdateFMODTaskLevel(taskLevel);
            }
        }
    }

    private void UpdateFMODTaskLevel(int taskNum)
    {
        if (taskNum == 2) // MakeBreakfast
        {
            FMODEvents.instance.UpdateTaskLevel(1); // Task Level 1
        }
        else if (taskNum == 4) // Shower
        {
            FMODEvents.instance.UpdateTaskLevel(2); // Task Level 2
        }
        else if (taskNum == 3) // WashLaundry
        {
            FMODEvents.instance.UpdateTaskLevel(3); // Task Level 3
        }
    }

    public Task GetCurrentTask()
    {
        return currentTask;
    }

    private void ChangeLevel(Levels newLvl)
    {
        _currLvl = newLvl;
    }

    void OnEnable()
    {
        Task.CompleteTaskEvent += completeTask;
        GameStateManager.LevelChangeEvent += ChangeLevel;
    }

    void OnDisable()
    {
        Task.CompleteTaskEvent -= completeTask;
        GameStateManager.LevelChangeEvent -= ChangeLevel;
    }
}
