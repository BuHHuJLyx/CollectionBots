using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Map _map;

    private FlagController _selectedBase;

    private void OnEnable()
    {
        _inputReader.BaseClicked += OnBaseClicked;
        _inputReader.GroundClicked += OnGroundClicked;
    }

    private void OnBaseClicked(FlagController flagController)
    {
        _selectedBase = flagController;
    }

    private void OnGroundClicked(Vector3 position)
    {
        if (_selectedBase == null)
            return;
        
        if (_map.Contains(position))
            _selectedBase.Place(position);

        _selectedBase = null;
    }
}