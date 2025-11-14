using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace P4
{
    public class CollectableShield : MonoBehaviour
    {
        [SerializeField] private int shieldPoints = 10;
        [SerializeField] private bool duplicatePoints = false;
        [SerializeField] private bool growInSize = false;
        [SerializeField] private bool isSpecial = false;
        [SerializeField] private int pointThreshold = 100;
        [SerializeField] private bool moveCloserToSoldier = true;
        [SerializeField] private Transform assignedSoldier;
        [SerializeField] protected float speed = 10f;
        protected IEnumerator _currentRoutine;

        public delegate void OnSpecialShieldCollected();
        public static event OnSpecialShieldCollected SpecialShieldCollectedEvent;

        
        protected virtual void OnEnable()
        {
            if (duplicatePoints)
            {
                PointManager.PointsUpdatedEvent += HandleDuplicatePoints;
            }
            if (growInSize)
            {
                PointManager.PointsUpdatedEvent += HandleGrowth;
            }
            SpecialShieldCollectedEvent += MoveTowardsSoldier;
        }

        private void MoveTowardsSoldier()
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _currentRoutine = MoveCoroutine(assignedSoldier);
            StartCoroutine(_currentRoutine);
        }
        
        private IEnumerator MoveCoroutine(Transform target)
        {
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                MoveTowardsTarget(target);
                yield return new WaitForFixedUpdate();
            }
        }

        private void MoveTowardsTarget(Transform target)
        {
            var direction = (target.position - transform.position).normalized * (moveCloserToSoldier ? 1 : -1);
            transform.position += direction * speed * Time.deltaTime;
        }

        private void HandleDuplicatePoints(int points)
        {
            if (points % pointThreshold != 0) return;
            shieldPoints *= 2;
        }

        private void HandleGrowth(int points)
        {
            if (points % pointThreshold != 0) return;
            transform.localScale *= 1.5f;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("MagicCube"))
            {
                NotifyPointUpdate();
                if (isSpecial)
                {
                    SpecialShieldCollectedEvent?.Invoke();
                }
                gameObject.SetActive(false);
            }
        }
        
        private void NotifyPointUpdate()
        {
            PointManager.AddPoints(shieldPoints);
        }
    }
}