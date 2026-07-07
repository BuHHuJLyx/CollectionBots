using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private const int MaxAttempts = 10;
    
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _delay = 3f;
    
    [Header("Spawn Settings")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;
    
    [Header("Obstacle Check")]
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _obstacleLayer;
    
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            for (int i = 0; i < MaxAttempts; i++)
            {
                Vector3 position = GetRandomPosition();

                if (Physics.OverlapSphere(position, _radius, _obstacleLayer).Length == 0)
                {
                    Instantiate(_prefab, position, Quaternion.identity);
                    break;
                }
            }
            
            yield return _wait;
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(_minX, _maxX);
        float z = Random.Range(_minZ, _maxZ);
        
        return new Vector3(x, 1, z);
    }
}