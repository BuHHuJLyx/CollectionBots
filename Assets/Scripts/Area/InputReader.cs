using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private LayerMask _baseLayer;
    [SerializeField] private LayerMask _groundLayer;
    
    private Camera _camera;

    public event Action<FlagController> BaseClicked;
    public event Action<Vector3> GroundClicked;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit baseHit, Mathf.Infinity, _baseLayer))
            {
                if (baseHit.collider.TryGetComponent(out FlagController flagController))
                {
                    BaseClicked?.Invoke(flagController);
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit groundHit, Mathf.Infinity, _groundLayer))
            {
                GroundClicked?.Invoke(groundHit.point);
            }
        }
    }
}