using System;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Task currentTask;
    [SerializeField] private GameStateManager gameStateManager;
    public static event Action<int> MoveToNextTask;
    private Levels _currLvl;

    private float taskLevel = 0f;

    void Start()
    {
        gameStateManager = FindAnyObjectByType<GameStateManager>();
        _currLvl = gameStateManager.GetLevelState();

        if (_currLvl == Levels.Tutorial)
        {
            currentTask = gameObject.GetComponentInChildren<MakeBedTask>();
        }
        else
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
            if (_currLvl != Levels.Tutorial)
            {
                UpdateFMODTaskLevel(currentTask.GetTasknum());
            }

            currentTask = currentTask.GetNextTask();
            if (currentTask != null)
            {
                MoveToNextTask?.Invoke(currentTask.GetTasknum());
                currentTask.SetAsCurrent();
            }
        }
    }

    private void UpdateFMODTaskLevel(int taskNum)
    {
        float targetLevel = taskLevel;

        if (taskNum == 2) targetLevel = 1f;     // MakeBreakfast
        else if (taskNum == 4) targetLevel = 2f; // Shower
        else if (taskNum == 5) targetLevel = 3f; // WashLaundry

        if (Math.Abs(targetLevel - taskLevel) > 0.01f) // avoid unnecessary coroutine
        {
            StopAllCoroutines();
            StartCoroutine(SmoothTaskLevelTransition(targetLevel, 3f));
        }
    }

    private System.Collections.IEnumerator SmoothTaskLevelTransition(float targetLevel, float duration)
    {
        float startLevel = taskLevel;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            taskLevel = Mathf.Lerp(startLevel, targetLevel, elapsed / duration);
            AudioManager.instance.UpdateTaskLevel(taskLevel);
            yield return null;
        }

        taskLevel = targetLevel;
        AudioManager.instance.UpdateTaskLevel(taskLevel);
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
