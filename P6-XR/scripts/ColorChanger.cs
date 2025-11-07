using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Color changeToColor = Color.red;
    
    private Renderer _renderer;
    private Color _originalColor;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _originalColor = _renderer.material.color;
    }
    
    public void ChangeColor()
    {
        Debug.Log("Changing color");
        if (_renderer != null)
        {
            _renderer.material.color = changeToColor;
        }
    }
    
    public void ResetColor()
    {
        Debug.Log("Resetting color");
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }
}
