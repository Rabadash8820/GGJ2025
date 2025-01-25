using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    [RequireComponent(typeof(SphereCollider))]
    public class BubbleCollector : MonoBehaviour
    {
        private const float PI_X4 = Mathf.PI * 4f;
        private const float TRIGGER_OFFSET = 0.02f;

        private SphereCollider _sphereCollider;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public GameObject ScaledBubbleObject;

        public UnityEvent BubbleCollected = new();

        [ShowInInspector, ReadOnly]
        public float CurrentRadius => _sphereCollider.radius - TRIGGER_OFFSET;

        private void Awake() => _sphereCollider = GetComponent<SphereCollider>();

        private void OnTriggerEnter(Collider other)
        {
            Transform collectedParentTransform = other.attachedRigidbody.transform;
            CollectibleBubble collectibleBubble = collectedParentTransform.GetComponentInChildren<CollectibleBubble>();
            if (collectibleBubble == null)
                return;

            float oldBubbleRadius = CurrentRadius;
            float collectedSurfaceArea = getSphereSurfaceArea(collectibleBubble.SphereCollider.radius);
            float currBubbleSurfaceArea = getSphereSurfaceArea(oldBubbleRadius);
            float newBubbleRadius = Mathf.Sqrt((currBubbleSurfaceArea + collectedSurfaceArea) / PI_X4);
            _sphereCollider.radius = newBubbleRadius + TRIGGER_OFFSET;

            ScaledBubbleObject.transform.localScale = ScaledBubbleObject.transform.localScale * (newBubbleRadius / oldBubbleRadius);

            Debug.Log($"Collected bubble '{collectedParentTransform.name}' with radius {collectibleBubble.SphereCollider.radius}, new radius: {newBubbleRadius}");

            collectibleBubble.Collect();

            BubbleCollected.Invoke();
        }

        private float getSphereSurfaceArea(float radius) => PI_X4 * radius * radius;
    }
}
