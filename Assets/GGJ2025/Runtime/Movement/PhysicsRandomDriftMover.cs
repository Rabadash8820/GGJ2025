using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.Updating;

namespace GGJ2025.Movement
{
    public class PhysicsRandomDriftMover : Updatable
    {
        private struct DriftData
        {
            public DriftData(Vector3 vector, float timeRemaining)
            {
                Vector = vector;
                TimeRemaining = timeRemaining;
            }
            public Vector3 Vector { get; set; }
            public float TimeRemaining { get; set; }
        }

        private List<Rigidbody> _rigidbodies;
        private List<DriftData> _driftData;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform ParentOfRigidbodies;

        [Tooltip("Interpreted as a speed for kinematic rigidbodies, or an impulse for non-kinematic rigidbodies")]
        [MinMaxSlider(0.01f, 1000f, ShowFields = true)]
        public Vector2 MinMaxDriftMagnitude = new(10f, 50f);

        [MinMaxSlider(1f, 30f, ShowFields = true)]
        public Vector2 MinMaxDriftSeconds = new(1, 5);

        protected override void Awake()
        {
            base.Awake();

            _rigidbodies = ParentOfRigidbodies.GetComponentsInChildren<Rigidbody>().ToList();
            _driftData = Enumerable.Repeat(new DriftData(), _rigidbodies.Count).ToList();

            AddFixedUpdate(applyDrifts);
        }

        private void applyDrifts(float deltaTime)
        {
            for (int rb = 0; rb < _rigidbodies.Count; rb++) {
                var rigidbody = _rigidbodies[rb];
                if (rigidbody == null) {  // In case it or its GameObject were deleted
                    removeRigidbodyAt(rb);
                    continue;
                }

                DriftData driftData = _driftData[rb];
                if (rigidbody.isKinematic)
                    rigidbody.MovePosition(rigidbody.position +  driftData.Vector * deltaTime);

                driftData = new DriftData(driftData.Vector, driftData.TimeRemaining - deltaTime);
                _driftData[rb] = driftData;
                if (driftData.TimeRemaining <= 0)
                    resetDrift(rb);
            }
        }

        public void AddRigidbody(Rigidbody rigidbody)
        {
            _rigidbodies.Add(rigidbody);
            _driftData.Add(new DriftData());
        }

        public void RemoveRigidbody(Rigidbody rigidbody)
        {
            int index = _rigidbodies.FindIndex(rb => rb == rigidbody);
            removeRigidbodyAt(index);
        }

        private void removeRigidbodyAt(int index)
        {
            _rigidbodies[index] = _rigidbodies[^1];
            _rigidbodies.RemoveAt(_rigidbodies.Count - 1);

            _driftData[index] = _driftData[^1];
            _driftData.RemoveAt(_driftData.Count - 1);
        }

        private void resetDrift(int rigidbodyIndex)
        {
            float driftTime = Random.Range(MinMaxDriftSeconds.x, MinMaxDriftSeconds.y);

            var randCircleVect = Random.insideUnitCircle;
            var impulseDir = new Vector3(randCircleVect.x, 0f, randCircleVect.y);
            float impulseMag = Random.Range(MinMaxDriftMagnitude.x, MinMaxDriftMagnitude.y);
            Vector3 impulse = impulseMag * impulseDir;

            _driftData[rigidbodyIndex] = new DriftData(impulse, driftTime);
            
            if (!_rigidbodies[rigidbodyIndex].isKinematic)
                _rigidbodies[rigidbodyIndex].AddForce(impulse, ForceMode.Impulse);
        }
    }
}
