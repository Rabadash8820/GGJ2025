using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    [RequireComponent(typeof(SphereCollider))]
    public class PersonHitter : MonoBehaviour
    {
        public UnityEvent PersonHit = new();

        private void OnTriggerEnter(Collider other)
        {
            Transform hitParentTransform = other.attachedRigidbody.transform;
            PersonHead personHead = hitParentTransform.GetComponentInChildren<PersonHead>();
            if (personHead == null)
                return;

            Debug.Log($"Hit head of person '{hitParentTransform.name}'");

            PersonHit.Invoke();
        }
    }
}
