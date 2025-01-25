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
        public GameObject PopPrefab;

        public UnityEvent Collected = new();

        public void Collect()
        {
            _ = Instantiate(PopPrefab, transform.position, Quaternion.identity);
            Collected.Invoke();
            Destroy(gameObject);
        }
    }
}
