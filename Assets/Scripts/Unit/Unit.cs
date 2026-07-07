using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Unit : MonoBehaviour
{
    private Mover _mover;
    
    private Base _base;
    private Resource _resource;
    private UnitState _state;
    private Transform _currentTarget;
    
    public bool IsIdle => _state == UnitState.Idle;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void FixedUpdate()
    {
        if (_state == UnitState.Idle)
            return;
        
        if (_mover.Move(_currentTarget))
        {
         if (_state == UnitState.MoveToResource)
             Take();
         else if (_state == UnitState.MoveToBase)
             Put();
        }
    }

    public void AssignResource(Base targetBase, Resource resource)
    {
        if (targetBase == null)
            throw new ArgumentNullException(nameof(targetBase));

        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        _base = targetBase;
        _resource = resource;

        _currentTarget = resource.transform;
        _state = UnitState.MoveToResource;
    }

    private void Take()
    {
        _currentTarget = _base.transform;
        _state = UnitState.MoveToBase;
        
        _resource.Take(transform);
    }

    private void Put()
    {
        _resource.Drop();

        _base.ReceiveResource(_resource);

        _resource = null;
        _currentTarget = null;
        _state = UnitState.Idle;
    }
}