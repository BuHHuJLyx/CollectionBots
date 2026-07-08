using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(UnitController))]
public class Base : MonoBehaviour
{
    [SerializeField] private float _scanDelay = 2f;
    [SerializeField] private ResourceRepository _repository;

    private Storage _storage;
    private Scanner _scanner;
    private UnitController _unitController;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _scanner = GetComponent<Scanner>();
        _unitController = GetComponent<UnitController>();
        
        _wait = new WaitForSeconds(_scanDelay);
    }

    private void OnEnable()
    {
        _unitController.Subscribe(ReceiveResource);
    }

    private void Start()
    {
        StartCoroutine(WorkRoutine());
    }

    private void OnDisable()
    {
        _unitController.Unsubscribe(ReceiveResource);
    }

    private void ReceiveResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        
        _repository.Remove(resource);
        _storage.AddResource();
    }

    private IEnumerator WorkRoutine()
    {
        while (enabled)
        {
            foreach (var resource in _scanner.GetResources())
                _repository.Add(resource);
            
            Resource freeResource = _repository.GetFree();

            if (freeResource != null)
                if (_unitController.TryAssignResource(transform, freeResource) == false)
                    _repository.Return(freeResource);
            
            yield return _wait; 
        }
    }
}