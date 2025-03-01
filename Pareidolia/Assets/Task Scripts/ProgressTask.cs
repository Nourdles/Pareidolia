using System;
using UnityEngine;

public class ProgressTask : Task
{
    [SerializeField] private float _progress;
    [SerializeField] private bool _isCharging;
    [SerializeField] private float _chargeSpeed; // set in inspector
    public static event Action<float> UpdateProgressBarEvent;
    public static event Action<bool> UpdatePBVisibilityEvent;

    protected override void Start()
    {
        base.Start();
        _progress = 0f;
        _isCharging = false;
    }

    protected void Update()
    {
        bool pbvisible = false;
        // at any point if progress becomes 1, complete the task
        if (_progress == 1)
        {
            completeTask();
        } else if (_isCharging)
        {
            Charge();
            UpdateProgressBarEvent?.Invoke(_progress);
            pbvisible = true;
        }
        UpdatePBVisibilityEvent?.Invoke(pbvisible);
    }

    protected void Charge()
    {
        _progress += _chargeSpeed * Time.deltaTime;
        if (_progress > 1)
        {
            _progress = 1;
        }
    }

    protected void startCharging()
    {
        _isCharging = true;
    }

    protected void stopCharging()
    {
        _isCharging = false;
    }
}
