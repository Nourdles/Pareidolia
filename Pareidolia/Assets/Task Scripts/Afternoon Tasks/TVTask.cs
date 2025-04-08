using UnityEngine;

public class TVTask : SimpleTask
{
    protected override void Start()
    {
        base.Start();
        task = (int) Tasks.WatchTV;
        stringrepresentation = "watch some TV";
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
