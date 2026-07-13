using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private List<Unit> _units;

    public event Action<Resource> ResourceDelivered;
    public event Action<Unit> BuildCompleted;

    public bool CanSendBuilder => _units.Count > 1;
    public bool HasFreeUnit => FindFreeUnit() != null;

    private void Awake()
    {
        _units = new List<Unit>();
        
        foreach (Unit unit in GetComponentsInChildren<Unit>())
            Add(unit);
    }

    public bool TryAssignResource(Transform baseTransform, Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        Unit unit = FindFreeUnit();

        if (unit == null)
            return false;

        unit.AssignResource(baseTransform, resource);

        return true;
    }

    public bool TryAssignBuild(Transform buildPoint)
    {
        if (buildPoint == null)
            throw new ArgumentNullException(nameof(buildPoint));
        
        Unit unit = FindFreeUnit();

        if (unit == null)
            return false;

        unit.AssignBuildTask(buildPoint);

        return true;
    }

    public void Add(Unit unit)
    {
        if (unit == null)
            throw new ArgumentNullException(nameof(unit));

        _units.Add(unit);

        unit.ResourceDelivered += OnResourceDelivered;
        unit.BuildCompleted += OnBuildCompleted;
    }
    
    public void Remove(Unit unit)
    {
        if (unit == null)
            throw new ArgumentNullException(nameof(unit));

        unit.ResourceDelivered -= OnResourceDelivered;
        unit.BuildCompleted -= OnBuildCompleted;

        _units.Remove(unit);
    }

    private void OnBuildCompleted(Unit unit)
    {
        BuildCompleted?.Invoke(unit);
    }

    private void OnResourceDelivered(Resource resource)
    {
        ResourceDelivered?.Invoke(resource);
    }

    private Unit FindFreeUnit()
    {
        foreach (var unit in _units)
            if (unit.IsIdle)
                return unit;

        return null;
    }
}