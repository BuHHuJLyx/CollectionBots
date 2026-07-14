using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private ResourceRepository _repository;
    [SerializeField] private Vector3 _spawnPosition;

    [SerializeField] private int _startUnits = 3;

    private void Awake()
    {
        Base startBase = Instantiate(_basePrefab, _spawnPosition, Quaternion.identity);
        
        startBase.Initialize(_repository);

        UnitFactory factory = startBase.GetComponent<UnitFactory>();

        for (int i = 0; i < _startUnits; i++)
        {
            Unit unit = factory.Create();
            startBase.AddUnit(unit);
        }
    }
}