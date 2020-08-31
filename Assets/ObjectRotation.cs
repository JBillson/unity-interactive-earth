using System;
using Lean.Gui;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public LeanWindow baseWindow;
    private Vector3 _previousPos;
    private Vector3 _posDelta;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (baseWindow.On) return;
        if (Input.GetMouseButton(0))
        {
            _posDelta = (Input.mousePosition - _previousPos) * rotationSpeed;
            transform.Rotate(Vector3.up, Vector3.Dot(_posDelta, _camera.transform.right), Space.World);
            transform.Rotate(-_camera.transform.right, -Vector3.Dot(_posDelta, _camera.transform.up), Space.World);
        }

        _previousPos = Input.mousePosition;
    }
}