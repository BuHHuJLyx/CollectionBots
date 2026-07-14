using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Transform _spawnPoint;
    
    public Transform SpawnPoint => _spawnPoint;

    public Unit Create()
    {
        Unit unit = Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
        unit.transform.SetParent(transform);

        return unit;
    }
}