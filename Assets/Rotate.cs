using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform camera;
    public Transform plane;

    public float speed = 1;

    private void FixedUpdate()
    {
        var dirToPlane = plane.transform.position - transform.position;
        var dirToCamera = camera.transform.position - transform.position;

        Debug.DrawRay(Vector3.zero, dirToPlane, Color.red);
        Debug.DrawRay(Vector3.zero, dirToCamera, Color.red);

        // acos(dot product of Va & Vb) assuming that Va & Vb are normalized
        var theta = Math.Acos(Vector3.Dot(dirToPlane.normalized, dirToCamera.normalized));
        // cross product of Va & Vb
        var cross = Vector3.Cross(dirToPlane, dirToCamera);
        // if dot product of cross and Vn is negative theta is negative 
        if (Vector3.Dot(cross, Vector3.up) < 0)
            theta = -theta;

        // rotate world by theta
        transform.transform.RotateAround(transform.position, cross, (float)theta);
    }
}