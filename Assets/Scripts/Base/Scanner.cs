using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;
    
    private List<Resource> _resources;

    private void Awake()
    {
        _resources = new List<Resource>();
    }

    public void Scan()
    {
        _resources.Clear();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        foreach (var collider in colliders)
            if (collider.TryGetComponent(out Resource resource))
                _resources.Add(resource);
    }

    public Resource FindFreeResource()
    {
        foreach (var resource in _resources)
            if (resource.IsFree)
                return resource;
        
        return null;
    }
}