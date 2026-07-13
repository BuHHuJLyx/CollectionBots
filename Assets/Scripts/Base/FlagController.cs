using System;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _flag;
    
    public event Action FlagPlaced;
    public event Action FlagRemoved;
    
    public Transform Transform => _flag.transform;
    public bool HasFlag => _flag != null && _flag.IsPlaced;

    public void Place(Vector3 position)
    {
        if (_flag == null)
            _flag = Instantiate(_flagPrefab);
        
        _flag.SetPosition(position);
        
        FlagPlaced?.Invoke();
    }

    public void Remove()
    {
        if (_flag == null)
            return;
        
        _flag.Hide();
        
        FlagRemoved?.Invoke();
    }
}