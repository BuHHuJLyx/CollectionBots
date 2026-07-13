using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Unit : MonoBehaviour
{
    private Mover _mover;
    private Resource _resource;
    
    private UnitState _state;
    
    private Transform _currentTarget;
    private Transform _baseTransform;
    
    public event Action<Resource> ResourceDelivered;
    public event Action<Unit> BuildCompleted;

    public bool IsIdle => _state == UnitState.Idle;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        
        _state = UnitState.Idle;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case UnitState.Idle:
                return;

            case UnitState.MoveToResource:
                ProcessMove(_currentTarget, Take);
                break;

            case UnitState.MoveToBase:
                ProcessMove(_currentTarget, Put);
                break;

            case UnitState.MoveToBuilding:
                ProcessMove(_currentTarget, Build);
                break;
        }
    }

    public void AssignResource(Transform baseTransform, Resource resource)
    {
        if (baseTransform == null)
            throw new ArgumentNullException(nameof(baseTransform));
        
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        _baseTransform = baseTransform;
        _resource = resource;

        _currentTarget = resource.transform;
        _state = UnitState.MoveToResource;
    }
    
    public void AssignBuildTask(Transform buildPoint)
    {
        if (buildPoint == null)
            throw new ArgumentNullException(nameof(buildPoint));
        
        _currentTarget = buildPoint;
        _state = UnitState.MoveToBuilding;
    }
    
    private void ProcessMove(Transform target, Action onReached)
    {
        _mover.Move(target);

        if (_mover.ReachedTarget(target))
            onReached();
    }

    private void Take()
    {
        _currentTarget = _baseTransform;
        _state = UnitState.MoveToBase;
        
        _resource.Take(transform);
    }

    private void Put()
    {
        Resource resource = _resource;
        
        ResourceDelivered?.Invoke(resource);
        
        resource.Drop();

        _resource = null;
        _currentTarget = null;
        _state = UnitState.Idle;
    }
    
    private void Build()
    {
        _currentTarget = null;
        _state = UnitState.Idle;
        
        BuildCompleted?.Invoke(this);
    }
}