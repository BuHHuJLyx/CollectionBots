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

    public void Move(Transform target)
    {
        Vector3 direction = GetDirection(target);

        if (direction.sqrMagnitude > _stopDistanceSqr)
            _rigidbody.MovePosition(_rigidbody.position + direction.normalized * _speed * Time.fixedDeltaTime);

    }
    
    public bool ReachedTarget(Transform target)
    {
        return GetDirection(target).sqrMagnitude <= _stopDistanceSqr;
    }
    
    private Vector3 GetDirection(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        return target.position - _rigidbody.position;
    }
}