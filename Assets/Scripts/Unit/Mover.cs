using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _stopDistanceSqr;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool Move(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        Vector3 direction = target.position - _rigidbody.position;

        if (direction.sqrMagnitude <= _stopDistanceSqr)
            return true;
        
        _rigidbody.MovePosition(_rigidbody.position + direction.normalized * _speed * Time.fixedDeltaTime);

        return false;
    }
}