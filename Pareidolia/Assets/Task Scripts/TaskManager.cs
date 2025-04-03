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

        AudioManager.instance.UpdateTaskLevel(taskLevel);
    }

    private void completeTask()
    {
        if (currentTask != null)
        {
            currentTask = currentTask.GetNextTask(); // go to next task
            if (currentTask != null)
            {
                MoveToNextTask?.Invoke(currentTask.GetTasknum());
                currentTask.SetAsCurrent();

                if (_currLvl != Levels.Tutorial) {
                    UpdateFMODTaskLevel(currentTask.GetTasknum());
                }
            }
        }
    }

    private void UpdateFMODTaskLevel(int taskNum)
    {
        if (taskNum == 3) // MakeBreakfast
        {
            taskLevel = 1;
            AudioManager.instance.UpdateTaskLevel(taskLevel); // Task Level 1
        }
        else if (taskNum == 5) // Shower
        {
            taskLevel = 2;
            AudioManager.instance.UpdateTaskLevel(taskLevel); // Task Level 2
        }
        else if (taskNum == 6) // WashLaundry
        {
            taskLevel = 3;
            AudioManager.instance.UpdateTaskLevel(taskLevel); // Task Level 3
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
