using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private Map _map;

    private FlagController _selectedBase;

    private void OnEnable()
    {
        _raycaster.BaseClicked += OnBaseClicked;
        _raycaster.GroundClicked += OnGroundClicked;
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
            _selectedBase.PlaceFlag(position);

        _selectedBase = null;
    }
}