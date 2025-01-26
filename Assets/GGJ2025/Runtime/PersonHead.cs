using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class PersonHead : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public bool IsHit { get; private set; }

        public UnityEvent BeingHit = new();

        public void Hit()
        {
            if (IsHit)
                return;

            IsHit = true;
            BeingHit.Invoke();
        }
    }
}
