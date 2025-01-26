using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    [RequireComponent(typeof(SphereCollider))]
    public class BubbleCollector : MonoBehaviour
    {
        private const float PI_X4 = Mathf.PI * 4f;
        private const float TRIGGER_OFFSET = 0.05f;

        private SphereCollider _sphereCollider;

        [ShowInInspector, ReadOnly]
        public float CurrentRadius => _sphereCollider == null ? 0f : _sphereCollider.radius - TRIGGER_OFFSET;

        [Tooltip("X-axis is current radius, y-axis is growth increment at that radius")]
        public AnimationCurve RadiusGrowthCurve;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public GameObject ScaledBubbleObject;

        public UnityEvent BubbleCollected = new();

        private void Awake() => _sphereCollider = GetComponent<SphereCollider>();

        private void OnTriggerEnter(Collider other)
        {
            Transform collectedParentTransform = other.attachedRigidbody == null ? null : other.attachedRigidbody.transform;
            CollectibleBubble collectibleBubble = collectedParentTransform == null ? null : collectedParentTransform.GetComponentInChildren<CollectibleBubble>();
            if (collectibleBubble == null)
                return;

            float increment = RadiusGrowthCurve.Evaluate(CurrentRadius);

            float oldBubbleRadius = CurrentRadius;
            float newBubbleRadius = oldBubbleRadius + increment;
            _sphereCollider.radius = newBubbleRadius + TRIGGER_OFFSET;

            ScaledBubbleObject.transform.localScale = ScaledBubbleObject.transform.localScale * (newBubbleRadius / oldBubbleRadius);

            Debug.Log($"Collected bubble '{collectedParentTransform.name}', growing by {increment} to new radius: {newBubbleRadius}");

            collectibleBubble.Collect();

            BubbleCollected.Invoke();
        }

        private float getSphereSurfaceArea(float radius) => PI_X4 * radius * radius;
    }
}
