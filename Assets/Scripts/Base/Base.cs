using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _scanDelay = 2f;
    [SerializeField] private ResourceRepository _repository;

    private Storage _storage;
    private Scanner _scanner;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _scanner = GetComponent<Scanner>();
        
        _wait = new WaitForSeconds(_scanDelay);
    }

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    public void ReceiveResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        _repository.Remove(resource);
        _storage.AddResource();
        resource.Die();
    }

    private IEnumerator ScanRoutine()
    {
        while (enabled)
        {
            _scanner.Scan();

            foreach (var unit in _units)
            {
                if (unit.IsIdle)
                {
                    Resource freeResource = _repository.GetFree();

                    if (freeResource == null)
                        break;
                
                    unit.AssignResource(this, freeResource);
                }
            }
        
            yield return _wait; 
        }
    }
}