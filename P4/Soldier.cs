using System.Collections;
using UnityEngine;

namespace P4
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField] private Transform baseTarget;
        [SerializeField] private CollisionNotifier collisionNotifier;
        [SerializeField] protected float speed;
        protected Rigidbody Rb;
        protected IEnumerator _currentRoutine;

        protected virtual void Start()
        {
            Rb = GetComponent<Rigidbody>();
            if (collisionNotifier != null) 
            {
                collisionNotifier.CollisionEvent += StartMovement;
            }
        }
        
        protected void StartMovement(Transform target)
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _currentRoutine = MoveCoroutine(target);
            StartCoroutine(_currentRoutine);
        }

        public void StartMovement()
        {
            StartMovement(baseTarget);
        }
        
        private void MoveTowardsTarget(Transform target)
        {
            if (baseTarget == null) return;
            var direction = (target.position - transform.position).normalized;
            Rb.linearVelocity = direction * speed;
        }

        private IEnumerator MoveCoroutine(Transform target)
        {
            while (Vector3.Distance(transform.position, baseTarget.position) > 0.1f)
            {
                MoveTowardsTarget(target);
                yield return new WaitForFixedUpdate();
            }
            Rb.linearVelocity = Vector3.zero;
        }
    }
}