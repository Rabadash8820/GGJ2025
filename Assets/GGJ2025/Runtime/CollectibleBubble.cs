using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class CollectibleBubble : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public SphereCollider SphereCollider;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform PopEffects;

        public UnityEvent Collected = new();

        public void Collect()
        {
            // Re-parent the effects so they're still there after this object is destroyed
            PopEffects.parent = transform.parent;

            Collected.Invoke();
            Destroy(gameObject);
        }
    }
}
