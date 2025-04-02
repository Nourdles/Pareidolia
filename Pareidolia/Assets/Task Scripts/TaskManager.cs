using System;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Task currentTask;
    [SerializeField] private GameStateManager gameStateManager;
    public static event Action<int> MoveToNextTask;
    private Levels _currLvl;

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
            }
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
