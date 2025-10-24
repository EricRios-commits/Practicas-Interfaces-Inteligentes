using UnityEngine;
using UnityEngine.InputSystem;

namespace P5
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private float speed = 5f;
        private Rigidbody _rigidbody;

        private void OnEnable()
        {
            moveAction?.action?.Enable();
        }

        private void OnDisable()
        {
            moveAction?.action?.Disable();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Camera.main == null) return;
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();

            Vector2 move2 = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
            Vector3 movement = forward * move2.y + right * move2.x;
            if (movement.sqrMagnitude > 1f) movement.Normalize();

            Vector3 newPosition = _rigidbody.position + movement * (speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
        }
    }
}