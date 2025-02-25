using UnityEngine;

public class ShowerTask : ProgressTask
{

    protected override void Start()
    {
        base.Start();
        task = Tasks.Shower;
    }
}
