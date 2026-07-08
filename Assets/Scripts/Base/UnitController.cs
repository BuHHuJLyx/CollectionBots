using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    
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
    
    public void Subscribe(Action<Resource> onReceiveResource)
    {
        foreach (var unit in _units)
            unit.ResourceDelivered += onReceiveResource;
    }
    
    public void Unsubscribe(Action<Resource> onReceiveResource)
    {
        foreach (var unit in _units)
            unit.ResourceDelivered -= onReceiveResource;
    }
    
    private Unit FindFreeUnit()
    {
        foreach (var unit in _units)
            if (unit.IsIdle)
                return unit;

        return null;
    }

}