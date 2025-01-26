using Sirenix.OdinInspector;
using UnityEngine;

namespace GGJ2025
{
    public class PersonHead : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public bool IsHit { get; private set; }

        public void Hit()
        {
            IsHit = true;
        }
    }
}
