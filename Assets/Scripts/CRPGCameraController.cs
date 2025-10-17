using System;
using DefaultNamespace;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CRPGCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera _normalCamera;
    [SerializeField] CinemachineCamera _tacticalCamera;
    
    [SerializeField] Transform _player;
    [SerializeField] float _raycastSourcePointY = 200f;
    [SerializeField] float _raycastMaxDistanceY = 800f;
    [SerializeField] LayerMask _layerMask;
    
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _shiftedMoveSpeed = 10f;
    [SerializeField] float _tacticalMoveSpeed = 5f;
    [SerializeField] float _shiftedTacticalMoveSpeed = 10f;
    
    [SerializeField] InputActionReference _move;
    [SerializeField] InputActionReference _shift;
    [SerializeField] InputActionReference _focus;
    [SerializeField] InputActionReference _tactical;
    [SerializeField] InputActionReference _zoom;

    Ray _ray;

    bool IsInTacticalMode
    {
        get => _isTactical;
        set => SetTacticalCamera(value);
    }
    bool _isTactical;

    float MoveSpeed =>
        (IsInTacticalMode, _shift.action.IsPressed()) switch
        {
            (true, true) => _shiftedTacticalMoveSpeed,
            (true, false) => _tacticalMoveSpeed,
            (false, true) => _shiftedMoveSpeed,
            (false, false) => _moveSpeed
        };
    
    void Start()
    {
        _ray = new()
        {
            origin = Vector3.zero,
            direction = Vector3.down
        };
        
        _focus.action.performed += HandleFocus;
        _tactical.action.performed += HandleTactical;

        SetTacticalCamera(false);
    }

    void OnDestroy()
    {
        _focus.action.performed -= HandleFocus;
        _tactical.action.performed -= HandleTactical;
    }
    
    void HandleFocus(InputAction.CallbackContext ctx)
    {
        if (transform.IsChildOf(_player))
        {
            return;
        }
        transform.SetParent(_player);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void HandleTactical(InputAction.CallbackContext ctx)
    {
        IsInTacticalMode = !IsInTacticalMode;
    }

    void HandleMove()
    {
        var pan2d = _move.action.ReadValue<Vector2>();
        if (pan2d.magnitude < 0.01f)
        {
            return;
        }
        if (transform.IsChildOf(_player))
        {
            transform.SetParent(null);
        }
        var pan = Time.deltaTime * MoveSpeed * (_normalCamera.transform.rotation.With(0f, VecParam.X).With(0, VecParam.Z) * pan2d.ToVec3(VecParam.X, VecParam.Z));
        transform.position += pan;
    }

    void Update()
    {
        HandleMove();
    }

    readonly RaycastHit[] _hits = new RaycastHit[12];

    void FixedUpdate()
    {
        _ray.origin = transform.position.With(_raycastSourcePointY, VecParam.Y);
        int count = Physics.RaycastNonAlloc(_ray, _hits, _raycastMaxDistanceY, _layerMask);
        
        if (count == 0)
        {
            return;
        }

        var sliceOfHits = _hits[..count];
        Array.Sort(sliceOfHits, (a,b) => Mathf.Abs(_player.position.y - a.point.y).CompareTo(Mathf.Abs(_player.position.y - b.point.y)));
        transform.position = sliceOfHits[0].point;
        
        if (transform.parent)
        {
            transform.localRotation = Quaternion.Inverse(transform.parent.rotation);
        }
    }

    void SetTacticalCamera(bool active)
    {
        _isTactical = active;
        _tacticalCamera.Priority.Value = active ? 1 : 0;
        _normalCamera.Priority.Value = active ? 0 : 1;
    }
}
