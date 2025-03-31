using System;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Task currentTask;
    public static event Action<int> MoveToNextTask;
    private Levels _currLvl;

    void Start()
    {
        if (_currLvl == Levels.Tutorial)
        {
            currentTask = GetComponentInChildren<MakeBedTask>();
        } else
        {
            currentTask = GetComponentInChildren<MakeCoffeeTask>();
        }
        
    }

    private void completeTask()
    {
        if (currentTask != null)
        {
            currentTask = currentTask.GetNextTask(); // go to next task
            if (currentTask != null)
            {
                MoveToNextTask?.Invoke(currentTask.GetTasknum());
            }
            currentTask.SetAsCurrent();
        }
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
