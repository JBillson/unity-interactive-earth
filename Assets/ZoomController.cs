using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineTrackedDolly _trackedDolly;

    public float zoomSpeed;

    private Coroutine _zoomCoroutine;
    private bool _isZoomed;

    private void Awake()
    {
        _trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    public void ToggleZoom()
    {
        if (_zoomCoroutine != null)
            StopCoroutine(_zoomCoroutine);

        _zoomCoroutine = StartCoroutine(_isZoomed ? DoZoom(0) : DoZoom(1));

        _isZoomed = !_isZoomed;
    }

    public IEnumerator DoZoom(float positionToZoomTo)
    {
        Debug.Log("Started Camera Move");
        if (Math.Abs(positionToZoomTo - 1) < .01f)
            positionToZoomTo = _trackedDolly.m_Path.MaxPos;

        if (positionToZoomTo > _trackedDolly.m_PathPosition)
        {
            while (positionToZoomTo > _trackedDolly.m_PathPosition)
            {
                _trackedDolly.m_PathPosition += Time.deltaTime * zoomSpeed;
                yield return null;
            }
        }
        else
        {
            while (positionToZoomTo < _trackedDolly.m_PathPosition)
            {
                _trackedDolly.m_PathPosition -= Time.deltaTime * zoomSpeed;
                yield return null;
            }
        }
        Debug.Log("Finished Camera Move");
    }
}
