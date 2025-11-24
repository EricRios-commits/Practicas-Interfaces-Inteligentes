using UnityEngine;

namespace P4
{
    public class EventColorChanger : MonoBehaviour
    {
        [SerializeField] private CollisionNotifier collisionNotifier;
        [SerializeField] private Color color = Color.red;
        private Renderer _renderer;
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            collisionNotifier.CollisionEvent += ChangeColor;
        }
        
        private void ChangeColor()
        {
            if (_renderer != null)
            {
                _renderer.material.color = color;
            }
        }
    }
}