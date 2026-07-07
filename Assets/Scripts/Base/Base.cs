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
                    Resource freeResource = _scanner.FindFreeResource();

                    if (freeResource == null)
                        break;
                
                    freeResource.Reserve();
                
                    unit.AssignResource(this, freeResource);
                }
            }
        
            yield return _wait; 
        }
    }
}