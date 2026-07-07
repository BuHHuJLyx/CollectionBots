using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public event Action<int> ResourceCollected;
    
    public int Resources { get; private set; }

    public void AddResource()
    {
        Resources++;
        ResourceCollected?.Invoke(Resources);
    }
}