using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private const int MaxAttempts = 10;

    [SerializeField] private Resource _prefab;
    [SerializeField] private Map _map;
    [SerializeField] private float _delay = 3f;
    
    [Header("Obstacle Check")]
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _obstacleLayer;
    
    [Header("Pool Settings")]
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 30;
    
    private WaitForSeconds _wait;
    
    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);

        _pool = new ObjectPool<Resource>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (resource) => SpawnResource(resource),
            actionOnRelease: (resource) => resource.gameObject.SetActive(false),
            actionOnDestroy: (resource) => Destroy(resource.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }
    
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    public void ReturnResource(Resource resource)
    {
        resource.Collected -= ReturnResource;
        _pool.Release(resource);
    }
    
    private void SpawnResource(Resource resource)
    {
        for (int i = 0; i < MaxAttempts; i++)
        {
            Vector3 position = _map.GetRandomPosition();

            if (Physics.OverlapSphere(position, _radius, _obstacleLayer).Length == 0)
            {
                resource.gameObject.SetActive(true);
                resource.transform.position = position;
                resource.transform.rotation = Quaternion.identity;
                
                break;
            }
        }
        
        resource.Collected += ReturnResource;
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            _pool.Get();
            yield return _wait;
        }
    }
}