using UnityEngine;

public class TVTask : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) AfternoonTasks.WatchTV;
    }

    void OnEnable()
    {
        TVSceneManager.TVWatchedEvent += completeTask;
    }

    void OnDisable()
    {
        TVSceneManager.TVWatchedEvent -= completeTask;
    }
}
