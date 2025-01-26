using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025.Movement
{
    public class DiscreteHeightTransformMover : MonoBehaviour
    {
        private InputSystemActions _inputSystemActions;

        private int _currentHeightIndex;

        [PropertyRange(0d, "@" + nameof(Heights) + ".Length - 1")]
        public int StartHeightIndex = 1;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform Transform;

        public float[] Heights = new float[] { 2f, 5.5f, 8f };

        public UnityEvent MovingUp = new();
        public UnityEvent MovingDown = new();

        private void Awake()
        {
            _inputSystemActions = new();
            _inputSystemActions.Player.MoveUp.performed += ctx => hover(1);
            _inputSystemActions.Player.MoveDown.performed += ctx => hover(-1);
        }

        private void OnEnable() => _inputSystemActions.Player.Enable();

        private void OnDisable() => _inputSystemActions.Player.Disable();

        private void OnDestroy() => _inputSystemActions.Dispose();

        private void hover(int delta)
        {
            int prevHeightIndex = _currentHeightIndex;
            _currentHeightIndex = (int)Mathf.Clamp(prevHeightIndex + delta, 0f, Heights.Length - 1);
            if (_currentHeightIndex == prevHeightIndex)
                return;

            Transform.position = new Vector3(Transform.position.x, Heights[_currentHeightIndex], Transform.position.z);
            Debug.Log($"Moving {(delta > 0 ? "up" : "down")} from height {prevHeightIndex} ({Heights[prevHeightIndex]}) to {_currentHeightIndex} ({Heights[_currentHeightIndex]})");
            (delta > 0 ? MovingUp : MovingDown)?.Invoke();
        }

    }
}
