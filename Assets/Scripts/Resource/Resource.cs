using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    
    public event Action<Resource> Collected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Take(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.up;

        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }

        if (_collider != null)
            _collider.enabled = false;
    }
    
    public void Drop()
    {
        transform.SetParent(null);
        
        Collected?.Invoke(this);

        if (_rigidbody != null)
            _rigidbody.isKinematic = false;

        if (_collider != null)
            _collider.enabled = true;
    }
}