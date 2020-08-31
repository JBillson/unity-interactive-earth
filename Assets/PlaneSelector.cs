using UnityEngine;

public class PlaneSelector : MonoBehaviour
{
    private Camera _camera;
    private PlaneManager _planeManager;

    private void Awake()
    {
        _camera = Camera.main;
        _planeManager = FindObjectOfType<PlaneManager>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;
        if (!hit.collider.CompareTag("Plane")) return;
        var plane = hit.collider.GetComponent<Plane>();
        _planeManager.FocusPlane(plane);
    }
}
