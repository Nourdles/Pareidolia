using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private Levels _currLvl;
    private int _numMornComplete = 0;
    private int _numAfterComplete = 0;
    private int _numNightComplete = 0;
    private static int numMornTasks = 2;
    private static int numAfterTasks = 0;
    private static int numNightTasks = 0;
    

    public bool IsMorningComplete()
    {
        return _numMornComplete == numMornTasks;
    }

    public bool IsAfternoonComplete()
    {
        return _numAfterComplete == numAfterTasks;
    }

    public bool IsNightComplete()
    {
        return _numNightComplete == numNightTasks;
    }

    private void completeTask()
    {
        if (_currLvl == Levels.Morning || _currLvl == Levels.Tutorial)
        {
            _numMornComplete += 1;
        } else if (_currLvl == Levels.Afternoon)
        {
            _numAfterComplete += 1;
        } else
        {
            _numNightComplete += 1;
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
