using System;
using System.Collections;
using Lean.Gui;
using Lean.Transition;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [HideInInspector] public bool isFocusingPlane;
    public LeanWindow baseWindow;
    private Camera _camera;
    private ObjectRotation _objectRotation;
    private Plane _planeToFocus;
    private ZoomController _zoomController;
    private Coroutine _focusCoroutine;

    private void Awake()
    {
        _camera = Camera.main;
        _objectRotation = FindObjectOfType<ObjectRotation>();
        _zoomController = FindObjectOfType<ZoomController>();
    }

    public void FocusPlane(Plane plane)
    {
        if (isFocusingPlane)
        {
            if (plane.window.On)
                Unfocus();
            else
                ChangePlane(plane);
            return;
        }

        baseWindow.On = true;
        plane.window.On = true;
        _planeToFocus = plane;
        isFocusingPlane = true;
        _zoomController.ToggleZoom();
    }

    private void ChangePlane(Plane plane)
    {
        plane.window.On = true;
        _planeToFocus = plane;
    }

    private void Unfocus()
    {
        if (_planeToFocus == null) return;
        baseWindow.On = false;
        _zoomController.ToggleZoom();
        _planeToFocus = null;
        isFocusingPlane = false;
    }

    private void FixedUpdate()
    {
        if (!isFocusingPlane) return;
        if (_planeToFocus == null) return;
        DoFocusPlane();
    }

    private void DoFocusPlane()
    {
        // Ref: https://stackoverflow.com/questions/5188561/signed-angle-between-two-3d-vectors-with-same-origin-within-the-same-plane
        var dirToPlane = _planeToFocus.transform.position - transform.position;
        var dirToCamera = _camera.transform.position - transform.position;
        Debug.DrawRay(Vector3.zero, dirToPlane, Color.red);
        Debug.DrawRay(Vector3.zero, dirToCamera, Color.red);
        
        // acos(dot product of Va & Vb) assuming that Va & Vb are normalized
        var theta = Math.Acos(Vector3.Dot(dirToPlane.normalized, dirToCamera.normalized));
        // cross product of Va & Vb
        var cross = Vector3.Cross(dirToPlane, dirToCamera);

        // offset correction with help from XFallenEagle on WorldOfZero discord \\
        var planeRotation = _planeToFocus.GetComponent<PlaneRotation>();
        //Get the distance to the desired position
        var distance = (dirToPlane.normalized - dirToCamera.normalized).magnitude;
        //Check if any signs are negative (This is where the issue occurs where the plane is already offset when focusing)
        var sign = dirToPlane.normalized - dirToCamera.normalized;
        var negative = sign.x < 0 || sign.y < 0 || sign.z < 0 || planeRotation.direction == -1;
        //If we are far away from the object force speed to be 0.4
        if (negative && distance >  0.01f * planeRotation.flightSpeed/10) //0.01f is the tolerance, make it smaller to get closer to center 
            theta = Math.Max(theta, planeRotation.flightSpeed * 4 / 100);
        else
            theta = Math.Max(theta, planeRotation.flightSpeed*2 / 100 ); //Maintain position
        
        // rotate world by theta
        _objectRotation.transform.RotateAround(Vector3.zero, cross, (float) theta);
    }
}