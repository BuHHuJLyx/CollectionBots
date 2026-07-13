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
    
    public void ReturnResources(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Resources += amount;

        ResourceCollected?.Invoke(Resources);
    }

    public bool TrySpendResources(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        
        if (Resources < amount)
            return false;

        Resources -= amount;
        
        ResourceCollected?.Invoke(Resources);
        
        return true;
    }
}