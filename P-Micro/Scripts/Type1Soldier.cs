using UnityEngine;

namespace P4
{
    public class Type1Soldier : Soldier
    {
        public delegate void OnType1Collision();
        public static event OnType1Collision Type1CollisionEvent;

        [SerializeField] private string collisionTag;
        [SerializeField] private Transform type1Shield;
        [SerializeField] private Transform type2Shield;
        [SerializeField] private Transform teleportToShield;
        [SerializeField] private CollisionNotifier teleportNotifier;
        [SerializeField] private Renderer rend;
        [SerializeField] private AudioClip collisionSound;
        
        protected override void Start()
        {
            base.Start();
            Type1CollisionEvent += RespondToType1Collision;
            Type2Soldier.Type2CollisionEvent += RespondToType2Collision;
            teleportNotifier.CollisionEvent += TeleportToShield;
            if (teleportToShield == null)
            {
                teleportToShield = type1Shield;
            }
        }
        
        private void TeleportToShield()
        {
            Rb.position = teleportToShield.position;
        }
        
        private void RespondToType1Collision()
        {
            StartMovement(type2Shield);
        }
        
        private void RespondToType2Collision()
        {
            StartMovement(type1Shield);
        }
        
        private void OnCollisionEnter(Collision collision) 
        {
            var collidedTag = collision.gameObject.tag;
            if (!string.IsNullOrEmpty(collisionTag) && collidedTag == collisionTag) 
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);
                Type1CollisionEvent?.Invoke();
            }
            if (collision.gameObject.CompareTag("type2Shield"))
            {
                ChangeColor();
            }
        }

        private void ChangeColor()
        {
            rend.material.color = Color.black;
        }
    }
}