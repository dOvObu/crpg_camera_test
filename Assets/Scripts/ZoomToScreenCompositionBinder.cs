using Unity.Cinemachine;
using UnityEngine;

public class ZoomToScreenCompositionBinder : MonoBehaviour
{
    [SerializeField] RemappedZoomSource _zoomSource;
    [SerializeField] CinemachineRotationComposer _offsetTarget;
    
    void Start()
    {
        _zoomSource.Init();
        _offsetTarget.Composition.ScreenPosition.y = _zoomSource.CalculateValue();
    }

    void LateUpdate()
    {
        if (_zoomSource.HasChanged)
        {
            _offsetTarget.Composition.ScreenPosition.y = _zoomSource.CalculateValue();
        }
    }
}
