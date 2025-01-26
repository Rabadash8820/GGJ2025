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

        [ShowInInspector, ReadOnly]
        public float CurrentRadius => _sphereCollider == null ? 0f : _sphereCollider.radius - TRIGGER_OFFSET;

        public float RadiusShrinkSpeed = 0.05f;
        public float MinRadius = 0.1f;

        [Tooltip("X-axis is current radius, y-axis is growth increment at that radius")]
        public AnimationCurve RadiusGrowthCurve;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public GameObject ScaledBubbleObject;

        public UnityEvent BubbleCollected = new();
        public UnityEvent MinRadiusReached = new();

        protected override void Awake()
        {
            base.Awake();

            _sphereCollider = GetComponent<SphereCollider>();

            AddFixedUpdate(shrink);
        }

        private void OnTriggerEnter(Collider other)
        {
            Transform collectedParentTransform = other.attachedRigidbody == null ? null : other.attachedRigidbody.transform;
            CollectibleBubble collectibleBubble = collectedParentTransform == null ? null : collectedParentTransform.GetComponentInChildren<CollectibleBubble>();
            if (collectibleBubble == null)
                return;

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

            ScaledBubbleObject.transform.localScale = ScaledBubbleObject.transform.localScale * (newRadius / oldRadius);
        }

        private void shrink(float deltaTime)
        {
            float oldBubbleRadius = CurrentRadius;
            float newBubbleRadius = oldBubbleRadius - (RadiusShrinkSpeed * deltaTime);
            setRadius(oldBubbleRadius, newBubbleRadius);

            if (newBubbleRadius <= MinRadius) {
                Debug.Log("Bubble has reached min radius");
                RemoveFixedUpdate();
                MinRadiusReached.Invoke();
            }
        }
    }
}
