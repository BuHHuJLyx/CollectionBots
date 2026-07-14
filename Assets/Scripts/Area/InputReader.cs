using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private int _selectButton = 0;

    public event Action<Vector3> Clicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_selectButton))
            Clicked?.Invoke(Input.mousePosition);
    }
}