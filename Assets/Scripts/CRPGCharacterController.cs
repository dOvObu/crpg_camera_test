using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CRPGCharacterController : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] LayerMask _groundLayerMask = -1;
    [SerializeField] Camera _playerCamera;
    [SerializeField] InputActionReference _click;
    [SerializeField] NavMeshAgent _navMeshAgent;
    
    [Header("Visual Feedback")]
    [SerializeField] float _markerAchievedDistance = 0.1f;
    [SerializeField] GameObject _destinationMarkerPrefab;
    [SerializeField] Animator _animator;

    
    Vector3 _targetPosition;
    GameObject _currentDestinationMarker;
    
    int _currentCornerIndex = 0;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        if (_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }
        
        _navMeshAgent.updatePosition = true;
        _navMeshAgent.updateRotation = true;

        _click.action.started += HandleInput;
    }

    void OnDestroy()
    {
        _click.action.started -= HandleInput;
    }

    void Update()
    {
        _animator.SetFloat("speed", _navMeshAgent.velocity.sqrMagnitude / _navMeshAgent.speed);
    }

    void FixedUpdate()
    {
        if (IsCloseToMarker())
        {
            CompleteMovement();
        }
    }

    bool IsCloseToMarker()
    {
        return _currentDestinationMarker &&
               Vector3.Distance(transform.position, _currentDestinationMarker.transform.position) <= _markerAchievedDistance;
    }

    void HandleInput(InputAction.CallbackContext ctx)
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _groundLayerMask))
        {
            return;
        }

        if (SetDestination(hit.point))
        {
            ShowDestinationMarker(hit.point);
        }
    }
    
    bool SetDestination(Vector3 destination)
    {
        return _navMeshAgent.SetDestination(destination);
    }

    void CompleteMovement()
    {
        if (_currentDestinationMarker)
        {
            DestroyImmediate(_currentDestinationMarker);
            _currentDestinationMarker = null;
        }
    }

    void ShowDestinationMarker(Vector3 position)
    {
        if (_currentDestinationMarker != null)
        {
            DestroyImmediate(_currentDestinationMarker);
        }
        
        if (_destinationMarkerPrefab != null)
        {
            _currentDestinationMarker = Instantiate(_destinationMarkerPrefab, position, Quaternion.identity);
        }
    }
}
