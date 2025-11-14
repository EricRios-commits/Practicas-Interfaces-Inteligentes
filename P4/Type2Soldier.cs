using System.Collections;
using UnityEngine;

namespace P4
{
    public class Type2Soldier : Soldier
    {
        public delegate void OnType2Collision();
        public static event OnType2Collision Type2CollisionEvent;
        
        [SerializeField] private CollisionNotifier lookAtReferenceNotifier;
        [SerializeField] private Transform lookAtReference;
        
        [SerializeField] private string collisionTag;
        [SerializeField] private float distanceToAvoidTo = 25f;

        private Transform _player;

        protected override void Start()
        {
            base.Start();
            _player = GameObject.FindWithTag("MagicCube").transform;
            lookAtReferenceNotifier.CollisionEvent += LookAtReference;
            CollectableShield.SpecialShieldCollectedEvent += AvoidPlayer;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var collidedTag = collision.gameObject.tag;
            if (!string.IsNullOrEmpty(collisionTag) && collidedTag == collisionTag) 
            {
                Type2CollisionEvent?.Invoke();
            }
        }

        private void AvoidPlayer()
        {
            Debug.Log("AvoidPlayer");
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _currentRoutine = AvoidCoroutine(_player);
            StartCoroutine(_currentRoutine);
        }
        
        private IEnumerator AvoidCoroutine(Transform target)
        {
            while (Vector3.Distance(transform.position, target.position) < distanceToAvoidTo)
            {
                MoveAway(target);
                yield return new WaitForFixedUpdate();
            }
            Rb.linearVelocity = Vector3.zero;
        }

        private void MoveAway(Transform target)
        {
            var direction = transform.position - target.position;
            direction.y = 0;
            direction.Normalize();
            Rb.linearVelocity = direction * speed;
        }

        private void LookAtReference()
        {
            if (lookAtReference != null)
            {
                var direction = (lookAtReference.position - transform.position).normalized;
                var lookRotation = Quaternion.LookRotation(direction);
                Rb.MoveRotation(lookRotation);
            }
        }
    }
}