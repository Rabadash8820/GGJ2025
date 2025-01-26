using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityUtil.Updating;

namespace GGJ2025
{
    [RequireComponent(typeof(SphereCollider))]
    public class BubbleCollector : Updatable
    {
        private const float TRIGGER_OFFSET = 0.05f;

        private SphereCollider _sphereCollider;
        private Vector3 _initialBubbleScale;

        [ShowInInspector, ReadOnly, PropertyOrder(-1f)]
        public bool HasBubble { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-1f)]
        public float CurrentRadius => _sphereCollider == null ? 0f : _sphereCollider.radius - TRIGGER_OFFSET;

        [Tooltip("Initial radius after collecting a bubble")]
        public float InitialRadius = 1f;
        public float RadiusShrinkSpeed = 0.05f;
        public float MinRadius = 0.1f;

        [Tooltip("X-axis is current radius, y-axis is growth increment at that radius")]
        public AnimationCurve RadiusGrowthCurve;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public GameObject ScaledBubbleObject;

        public UnityEvent FirstBubbleCollected = new();
        public UnityEvent BubbleCollected = new();
        public UnityEvent MinRadiusReached = new();

        protected override void Awake()
        {
            base.Awake();

            _sphereCollider = GetComponent<SphereCollider>();
            _initialBubbleScale = ScaledBubbleObject.transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            Transform collectedParentTransform = other.attachedRigidbody == null ? null : other.attachedRigidbody.transform;
            CollectibleBubble collectibleBubble = collectedParentTransform == null ? null : collectedParentTransform.GetComponentInChildren<CollectibleBubble>();
            if (collectibleBubble == null)
                return;

            collectBubble(collectedParentTransform, collectibleBubble);
        }

        private void collectBubble(Transform collectedParentTransform, CollectibleBubble collectibleBubble)
        {
            if (!HasBubble) {
                setRadius(oldRadius: 0f, InitialRadius);
                HasBubble = true;
                Debug.Log("Collected new first bubble");
                FirstBubbleCollected.Invoke();
                AddFixedUpdate(shrink);
            }

            float oldBubbleRadius = CurrentRadius;
            float increment = RadiusGrowthCurve.Evaluate(oldBubbleRadius);

            float newBubbleRadius = oldBubbleRadius + increment;
            setRadius(oldBubbleRadius, newBubbleRadius);

            Debug.Log($"Collected bubble '{collectedParentTransform.name}', growing by {increment} to new radius: {newBubbleRadius}");

            collectibleBubble.Collect();

            BubbleCollected.Invoke();
        }

        private void setRadius(float oldRadius, float newRadius)
        {
            _sphereCollider.radius = newRadius + TRIGGER_OFFSET;

            ScaledBubbleObject.transform.localScale = HasBubble
                ? ScaledBubbleObject.transform.localScale * (newRadius / oldRadius)
                : _initialBubbleScale;
        }

        private void shrink(float deltaTime)
        {
            float oldBubbleRadius = CurrentRadius;
            float newBubbleRadius = oldBubbleRadius - (RadiusShrinkSpeed * deltaTime);
            setRadius(oldBubbleRadius, newBubbleRadius);

            if (newBubbleRadius <= MinRadius) {
                Debug.Log("Bubble has reached min radius");
                RemoveFixedUpdate();
                HasBubble = false;
                MinRadiusReached.Invoke();
            }
        }
    }
}
