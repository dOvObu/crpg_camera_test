using System;
using Unity.Cinemachine;
using UnityEngine;

public class ZoomToCameraOffsetBinder : MonoBehaviour
{
    [SerializeField] RemappedZoomSource _zoomSource;
    [SerializeField] CinemachineCameraOffset _offsetTarget;

    void Start()
    {
        _zoomSource.Init();
        _offsetTarget.Offset.y = _zoomSource.CalculateValue();
    }

    void LateUpdate()
    {
        if (_zoomSource.HasChanged)
        {
            _offsetTarget.Offset.y = _zoomSource.CalculateValue();
        }
    }
}
