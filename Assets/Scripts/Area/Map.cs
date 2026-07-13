using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Map : MonoBehaviour
{
    private Renderer _renderer;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public bool Contains(Vector3 position) =>
        _renderer.bounds.Contains(position);

    public Vector3 GetRandomPosition()
    {
        Bounds bounds = _renderer.bounds;
        
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        
        return new Vector3(x, bounds.max.y, z);
    }
}