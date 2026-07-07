using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;
    
    [SerializeField] private ResourceRepository _repository;

    public void Scan()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        foreach (var collider in colliders)
            if (collider.TryGetComponent(out Resource resource))
                _repository.Add(resource);
    }
}