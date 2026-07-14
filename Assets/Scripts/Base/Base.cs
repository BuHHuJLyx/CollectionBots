using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(UnitController))]
[RequireComponent(typeof(FlagController))]
[RequireComponent(typeof(UnitFactory))]
[RequireComponent(typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    private const int UnitResourceCost = 3;
    private const int BaseBuildCost = 5;

    [SerializeField] private float _scanDelay;

    private ResourceRepository _repository;
    private Storage _storage;
    private Scanner _scanner;
    private BaseBuilder _baseBuilder;
    private UnitFactory _unitFactory;
    private UnitController _unitController;
    private FlagController _flagController;

    private WaitForSeconds _wait;
    private BaseMode _mode;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _scanner = GetComponent<Scanner>();
        _baseBuilder = GetComponent<BaseBuilder>();
        _unitFactory = GetComponent<UnitFactory>();
        _unitController = GetComponent<UnitController>();
        _flagController = GetComponent<FlagController>();

        _wait = new WaitForSeconds(_scanDelay);
        _mode = BaseMode.CreateUnits;
    }

    private void OnEnable()
    {
        _unitController.ResourceDelivered += ReceiveResource;
        _unitController.BuildCompleted += OnBuildCompleted;

        _flagController.FlagPlaced += OnFlagPlaced;
        _flagController.FlagRemoved += OnFlagRemoved;
    }

    private void Start()
    {
        StartCoroutine(WorkRoutine());
    }

    private void OnDisable()
    {
        _unitController.ResourceDelivered -= ReceiveResource;
        _unitController.BuildCompleted -= OnBuildCompleted;

        _flagController.FlagPlaced -= OnFlagPlaced;
        _flagController.FlagRemoved -= OnFlagRemoved;
    }

    private void OnFlagPlaced()
    {
        _mode = BaseMode.BuildBase;
    }

    private void OnFlagRemoved()
    {
        _mode = BaseMode.CreateUnits;
    }

    public void Initialize(ResourceRepository repository)
    {
        _repository = repository;
    }

    public void AddUnit(Unit unit)
    {
        _unitController.Add(unit);
    }

    private void ReceiveResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        _repository.Remove(resource);
        _storage.AddResource();

        ProcessMode();
    }

    private void ProcessMode()
    {
        switch (_mode)
        {
            case BaseMode.CreateUnits:
                TryCreateUnit();
                break;

            case BaseMode.BuildBase:
                TryBuildBase();
                break;
        }
    }

    private void TryCreateUnit()
    {
        if (_storage.CanSpend(UnitResourceCost) == false)
            return;

        _storage.Spend(UnitResourceCost);

        Unit unit = _unitFactory.Create();
        _unitController.Add(unit);
    }

    private void TryBuildBase()
    {
        if (_flagController.HasFlag == false)
            return;

        if (_unitController.CanSendBuilder == false)
            return;

        if (_storage.CanSpend(BaseBuildCost) == false)
            return;
        
        if (_unitController.HasFreeUnit == false)
            return;

        _storage.Spend(BaseBuildCost);
        _unitController.TryAssignBuild(_flagController.Transform);
    }

    private void OnBuildCompleted(Unit unit)
    {
        Base newBase = _baseBuilder.Create(_flagController.Transform.position);
        newBase.Initialize(_repository);

        _unitController.Remove(unit);
        
        UnitFactory newFactory = newBase.GetComponent<UnitFactory>();
        unit.transform.SetParent(newBase.transform);
        unit.transform.position = newFactory.SpawnPoint.position;

        newBase.AddUnit(unit);

        _flagController.RemoveFlag();
    }

    private IEnumerator WorkRoutine()
    {
        while (enabled)
        {
            foreach (var resource in _scanner.GetResources())
                _repository.Add(resource);

            while (_unitController.HasFreeUnit)
            {
                Resource resourceToCollect = _repository.GetFree();

                if (resourceToCollect == null)
                    break;

                if (_unitController.TryAssignResource(transform, resourceToCollect) == false)
                {
                    _repository.Release(resourceToCollect);
                    break;
                }
            }

            yield return _wait;
        }
    }
}