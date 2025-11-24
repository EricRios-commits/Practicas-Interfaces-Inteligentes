using UnityEngine;

namespace P4
{
    public class CollisionNotifier : MonoBehaviour
    {
        public delegate void OnCollision();
        public event OnCollision CollisionEvent;
        
        [SerializeField] private string collisionTag;
        
        public void NotifyCollision()
        {
            CollisionEvent?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(other.gameObject.tag) && other.gameObject.CompareTag(collisionTag))
            {
                NotifyCollision();
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(collision.gameObject.tag) && collision.gameObject.CompareTag(collisionTag))
            {
                NotifyCollision();
            }
        }
    }
}