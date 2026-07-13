using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool IsPlaced => gameObject.activeSelf;
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}