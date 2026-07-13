using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    public Base Create(Vector3 position)
    {
        position.y = 0.5f;
        return Instantiate(_prefab, position, Quaternion.identity);
    }
}