using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.Updating;

namespace GGJ2025.Movement
{
    public class PhysicsPlaneRadialPusher : Updatable
    {
        private List<Rigidbody> _rigidbodies;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform CenterTransform;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform ParentOfRigidbodies;

        public Rigidbody[] ExtraRigidbodies = Array.Empty<Rigidbody>();

        [Min(0f)]
        public float MaxRadius = 5f;

        [Min(0f)]
        public float MaxHeightDelta = 0.01f;

        [MinMaxSlider(1f, 100f, ShowFields = true)]
        public Vector2 MinMaxForceMagnitude = new(10f, 50f);

        public bool DrawLineGizmos;

        protected override void Awake()
        {
            base.Awake();

            _rigidbodies = ParentOfRigidbodies.GetComponentsInChildren<Rigidbody>()
                .Concat(ExtraRigidbodies)
                .ToList();

            AddFixedUpdate(addForces);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null)
                return;

            _rigidbodies.Add(other.attachedRigidbody);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody == null)
                return;

            removeRigidbody(other.attachedRigidbody);
        }

        private void addForces(float deltaTime)
        {
            for (int rb = 0; rb < _rigidbodies.Count; rb++) {
                var rigidbody = _rigidbodies[rb];
                if (rigidbody == null) {  // In case it or its GameObject were deleted
                    removeRigidbody(rigidbody);
                    continue;
                }

                if (!rigidbody.gameObject.activeInHierarchy)
                    continue;

                if (Mathf.Abs(rigidbody.position.y - CenterTransform.position.y) > MaxHeightDelta)
                        continue;

                Vector3 vectFromCenter = (rigidbody.transform.position - CenterTransform.position);
                float distFromCenter = vectFromCenter.magnitude;
                if (distFromCenter > MaxRadius)
                    continue;

                Vector3 unitVectFromCenter = vectFromCenter / distFromCenter;
                unitVectFromCenter.y = 0f;  // Only push in the plane
                float forceMag = Mathf.Lerp(MinMaxForceMagnitude.y, MinMaxForceMagnitude.x, distFromCenter / MaxRadius);

                rigidbody.AddForce(forceMag * unitVectFromCenter, ForceMode.Force);
            }
        }

        private void removeRigidbody(Rigidbody rigidbody)
        {
            int index = _rigidbodies.FindIndex(rb => rb == rigidbody);
            if (index == -1)
                return;

            _rigidbodies[index] = _rigidbodies[^1];
            _rigidbodies.RemoveAt(_rigidbodies.Count - 1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CenterTransform.position, MaxRadius);

            if (!DrawLineGizmos || _rigidbodies is null)    // Null in edit mode...
                return;

            foreach (Rigidbody rigidbody in _rigidbodies) {
                if (rigidbody == null)
                    continue;

                Vector3 vectFromCenter = (rigidbody.transform.position - CenterTransform.position);
                float sqrDistFromCenter = vectFromCenter.sqrMagnitude;
                Gizmos.color = sqrDistFromCenter > MaxRadius * MaxRadius ? Color.yellow : Color.red;
                Gizmos.DrawLine(CenterTransform.position, rigidbody.position);
            }
        }
    }
}
