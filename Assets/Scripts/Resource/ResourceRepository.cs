using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRepository : MonoBehaviour
{
    private List<Resource> _freeResources;
    private List<Resource> _reservedResources;

    private void Awake()
    {
        _freeResources = new List<Resource>();
        _reservedResources = new List<Resource>();
    }

    public void Add(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        if (_freeResources.Contains(resource) || _reservedResources.Contains(resource))
            return;

        _freeResources.Add(resource);
    }

    public Resource GetFree()
    {
        if (_freeResources.Count == 0)
            return null;

        Resource resource = _freeResources[0];

        _freeResources.RemoveAt(0);
        _reservedResources.Add(resource);

        return resource;
    }
    
    public void Release(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        if (_reservedResources.Remove(resource))
            _freeResources.Add(resource);
    }

    public void Remove(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        _reservedResources.Remove(resource);
    }
}