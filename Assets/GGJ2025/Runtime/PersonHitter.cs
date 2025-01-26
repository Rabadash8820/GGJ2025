using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    [RequireComponent(typeof(SphereCollider))]
    public class PersonHitter : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public BubbleCollector BubbleCollector;

        public UnityEvent PersonHit = new();

        private void OnTriggerEnter(Collider other)
        {
            Transform hitParentTransform = other.attachedRigidbody == null ? null : other.attachedRigidbody.transform;
            PersonHead personHead = hitParentTransform == null ? null : hitParentTransform.GetComponentInChildren<PersonHead>();
            if (personHead == null)
                return;

            if (!BubbleCollector.HasBubble) {
                Debug.Log($"No bubble, so not hitting person head '{hitParentTransform.name}'", context: this);
                return;
            }

            if (personHead.IsHit) {
                Debug.Log($"Person head '{hitParentTransform.name}' has already been hit", context: this);
                return;
            }

            Debug.Log($"Hit person head '{hitParentTransform.name}'");
            personHead.Hit();
            PersonHit.Invoke();
        }
    }
}
