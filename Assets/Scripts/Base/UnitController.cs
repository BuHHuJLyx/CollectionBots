using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private List<Unit> _busyUnits;
    private Queue<Unit> _freeUnits;

    public event Action<Resource> ResourceDelivered;
    public event Action<Unit> BuildCompleted;

    public bool CanSendBuilder => _freeUnits.Count + _busyUnits.Count > 1;
    public bool HasFreeUnit => _freeUnits.Count > 0;

    private void Awake()
    {
        _busyUnits = new List<Unit>();
        _freeUnits = new Queue<Unit>();
    }

    public bool TryAssignResource(Transform baseTransform, Resource resource) =>
        TryAssign(unit => unit.AssignResource(baseTransform, resource));

    public bool TryAssignBuild(Transform buildPoint) =>
        TryAssign(unit => unit.AssignBuildTask(buildPoint));


    public void Add(Unit unit)
    {
        if (unit == null)
            throw new ArgumentNullException(nameof(unit));

        _freeUnits.Enqueue(unit);

        unit.ResourceDelivered += OnResourceDelivered;
        unit.BuildCompleted += OnBuildCompleted;
        unit.BecameIdle += OnUnitIdle;
    }
    
    public void Remove(Unit unit)
    {
        if (unit == null)
            throw new ArgumentNullException(nameof(unit));

        unit.ResourceDelivered -= OnResourceDelivered;
        unit.BuildCompleted -= OnBuildCompleted;
        unit.BecameIdle -= OnUnitIdle;

        _busyUnits.Remove(unit);
        RemoveFromFree(unit);
    }

    private void OnBuildCompleted(Unit unit)
    {
        BuildCompleted?.Invoke(unit);
    }

    private void OnResourceDelivered(Resource resource)
    {
        ResourceDelivered?.Invoke(resource);
    }
    
    private void OnUnitIdle(Unit unit)
    {
        _busyUnits.Remove(unit);
        _freeUnits.Enqueue(unit);
    }
    
    private void RemoveFromFree(Unit unit)
    {
        Queue<Unit> queue = new Queue<Unit>();

        while (_freeUnits.Count > 0)
        {
            Unit current = _freeUnits.Dequeue();

            if (current != unit)
                queue.Enqueue(current);
        }

        _freeUnits = queue;
    }
    
    private bool TryAssign(Action<Unit> assign)
    {
        if (_freeUnits.Count == 0)
            return false;

        Unit unit = _freeUnits.Dequeue();

        _busyUnits.Add(unit);

        assign(unit);

        return true;
    }
}