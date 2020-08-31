using System;
using System.Collections;
using UnityEngine;

public class PlaneRotation : MonoBehaviour
{
    public Transform objectToRotateAround;
    public float flightSpeed = 10f;
    [Range(-1, 1)] public int direction;

    private void FixedUpdate()
    {
        var t = transform;
        var dir = t.position - Vector3.zero;
        var left = Vector3.Cross(dir, t.right).normalized;
        t.RotateAround(Vector3.zero, -left * direction, Time.deltaTime * flightSpeed);
        t.LookAt(objectToRotateAround);
    }
}