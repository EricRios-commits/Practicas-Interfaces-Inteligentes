using System.Collections;
using UnityEngine;

namespace P5
{
    public class TreasureObject : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int collectionPoints = 1;
        public bool IsCollected { get; private set; }
        private Transform _playerTransform;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        public void OnPointerEnter()
        {
            MoveTowardsPlayer();
        }
        
        public void OnPointerExit()
        {
            StopMovement();
        }

        public void StopMovement()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                GetCollected();
            }
        }
        
        private void GetCollected()
        {
            NotifyPointUpdate();
            StopCoroutine(_moveCoroutine);
            IsCollected = true;
            gameObject.SetActive(false);
        }
        
        private void NotifyPointUpdate()
        {
            PointManager.AddPoints(collectionPoints);
        }
        
        public void MoveTowardsPlayer()
        {
            _moveCoroutine = StartCoroutine(MoveTowardsPlayerRoutine());
        }

        private IEnumerator MoveTowardsPlayerRoutine()
        {
            while (true)
            {
                Vector3 direction = (_playerTransform.position - transform.position).normalized;
                transform.position += direction * (speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}