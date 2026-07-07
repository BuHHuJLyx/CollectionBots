using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    
    private ResourceState _state;
    
    public bool IsFree => _state == ResourceState.Free;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        
        _state = ResourceState.Free;
    }

    public void Reserve()
    {
        _state = ResourceState.Reserved;
    }

    public void Take(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.up;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        if (_collider != null)
            _collider.enabled = false;
    }
    
    public void Drop()
    {
        transform.SetParent(null);

        if (_rigidbody != null)
            _rigidbody.isKinematic = false;

        if (_collider != null)
            _collider.enabled = true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}