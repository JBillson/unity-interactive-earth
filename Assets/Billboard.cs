using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        transform.LookAt(_camera.transform);
    }
}
