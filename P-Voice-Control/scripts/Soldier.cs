using System.Collections;
using UnityEngine;

namespace PVoz
{
    public class Soldier : MonoBehaviour, IVoiceControlled
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Stop1 = Animator.StringToHash("Stop");
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 5f;
        
        private Rigidbody _rb;
        private Animator _animator;
        private IEnumerator _currentRoutine;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void MoveForward()
        {
            StartMovement(transform.forward);
        }

        public void MoveBackward()
        {
            StartMovement(-transform.forward);
        }

        public void StopMovement()
        {
            _animator.SetTrigger(Stop1);
            Stop();
        }

        public void TurnLeft()
        {
            StartTurn(-rotateSpeed);
        }

        public void TurnRight()
        {
            StartTurn(rotateSpeed);
        }

        private void StartTurn(float speed)
        {
            Stop();
            _animator.SetTrigger(Turn);
            _currentRoutine = TurnCoroutine(speed);
            StartCoroutine(_currentRoutine);
        }
        
        private IEnumerator TurnCoroutine(float speed)
        {
            _rb.angularVelocity = Vector3.up * speed;
            yield return new WaitForFixedUpdate();
        }
        
        private void StartMovement(Vector3 direction)
        {
            Stop();
            _animator.SetTrigger(Walk);
            _currentRoutine = MoveCoroutine(direction);
            StartCoroutine(_currentRoutine);
        }

        private IEnumerator MoveCoroutine(Vector3 direction)
        {
            Move(direction);
            yield return new WaitForFixedUpdate();
        }

        private void Move(Vector3 direction)
        {
            _rb.linearVelocity = direction * moveSpeed;
        }
        
        private void Stop()
        {
            if (_currentRoutine != null) StopCoroutine(_currentRoutine);
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}