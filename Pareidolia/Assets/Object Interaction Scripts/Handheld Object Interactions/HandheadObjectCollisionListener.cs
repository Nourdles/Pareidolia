using UnityEngine;
using System;

public class HandheadObjectCollisionListener : MonoBehaviour
{
    public event Action<Vector3> OnObjectDropped; // Now an instance event

    private void OnCollisionEnter(Collision collision)
    {
        OnObjectDropped?.Invoke(transform.position);
    }
}