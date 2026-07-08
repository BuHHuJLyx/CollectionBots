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

    public bool IsIdle => _state == UnitState.Idle;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        
        _state = UnitState.Idle;
    }

    private void FixedUpdate()
    {
        if (_state == UnitState.Idle)
            return;
        
        _mover.Move(_currentTarget);
        
        if (_mover.ReachedTarget(_currentTarget) == false)
            return;

        switch (_state)
        {
            case UnitState.MoveToResource:
                Take();
                break;

            case UnitState.MoveToBase:
                Put();
                break;
        }
    }

    public void AssignResource(Transform baseTransform, Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        _baseTransform = baseTransform;
        _resource = resource;

        _currentTarget = resource.transform;
        _state = UnitState.MoveToResource;
    }

    private void Take()
    {
        _currentTarget = _baseTransform;
        _state = UnitState.MoveToBase;
        
        _resource.Take(transform);
    }

    private void Put()
    {
        _resource.Drop();

        ResourceDelivered?.Invoke(_resource);

        _resource = null;
        _currentTarget = null;
        _state = UnitState.Idle;
    }
}