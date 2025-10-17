using System;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class RemappedZoomSource
{
    [SerializeField] CinemachineOrbitalFollow _zoomSource;
    [SerializeField] float _top;
    [SerializeField] float _center;
    [SerializeField] float _bottom;
    
    readonly AnimationCurve _remap = new()
    {
        keys = new []
        {
            new Keyframe(0f  , 0f),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1f  , 1f)
        }
    };

    public bool HasChanged => _zoomSource.VerticalAxis.TrackValueChange();

    public void Init()
    {
        _remap.keys = new[]
        {
            new Keyframe(_zoomSource.VerticalAxis.Range.x, _bottom),
            new Keyframe(_zoomSource.VerticalAxis.Center, _center),
            new Keyframe(_zoomSource.VerticalAxis.Range.y, _top)
        };
    }
    
    public float CalculateValue()
    {
        return _remap.Evaluate(_zoomSource.VerticalAxis.Value);
    }
}
