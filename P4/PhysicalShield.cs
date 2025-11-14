using System.Collections;
using UnityEngine;

namespace P4
{
    public class PhysicalShield : CollectableShield
    {
        private Rigidbody _rigidbody;
        [SerializeField] private Transform enemySoldier;
        [SerializeField] private float pointToAlign;
        [SerializeField] private float distanceToAlign;

        protected override void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void MoveTo(Vector3 target)
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _currentRoutine = MoveCoroutine(target);
            StartCoroutine(_currentRoutine);
        }
        
        private void MoveTowardsTarget(Vector3 target)
        {
            var direction = (target - transform.position).normalized;
            _rigidbody.linearVelocity = direction * speed;
        }

        private IEnumerator MoveCoroutine(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                MoveTowardsTarget(target);
                yield return new WaitForFixedUpdate();
            }
            _rigidbody.linearVelocity = Vector3.zero;
        }

    }
}