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
    
    public bool CanSpend(int amount)
    {
        if (amount <= 0)
            return false;

        return Resources >= amount;
    }

    public void Spend(int amount)
    {
        Resources -= amount;
        ResourceCollected?.Invoke(Resources);
    }
}