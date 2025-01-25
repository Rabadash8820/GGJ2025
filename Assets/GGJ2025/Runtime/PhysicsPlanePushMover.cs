using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.Updating;

namespace GGJ2025
{
    public class PhysicsPlanePushMover : Updatable
    {
        private InputSystemActions _inputSystemActions;

        private Vector3 _moveVector = Vector3.zero;

        [Min(0f)]
        public float PushForce = 10.0f;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Rigidbody Rigidbody;

        protected override void Awake()
        {
            base.Awake();

            _inputSystemActions = new InputSystemActions();

            AddFixedUpdate(move);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _inputSystemActions.Player.Enable();
        }

        protected override void OnDisable()
        {
            base.OnEnable();
            _inputSystemActions.Player.Disable();
        }

        private void move(float deltaTime)
        {
            Vector2 inputVect = _inputSystemActions.Player.Move.ReadValue<Vector2>();
            _moveVector = new Vector3(inputVect.x, 0f, inputVect.y);
            Rigidbody.AddForce(_moveVector, ForceMode.Force);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _moveVector);
        }
    }
}
