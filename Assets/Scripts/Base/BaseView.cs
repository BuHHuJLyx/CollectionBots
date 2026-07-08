using TMPro;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private Storage _storage;

    private void OnEnable()
    {
        _storage.ResourceCollected += OnResourceCollected;
    }

    private void OnDisable()
    {
        _storage.ResourceCollected -= OnResourceCollected;
    }

    private void OnResourceCollected(int resources)
    {
        _score.text = resources.ToString();
    }
}