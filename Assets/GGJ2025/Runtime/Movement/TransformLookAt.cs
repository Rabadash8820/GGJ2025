using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.Updating;

namespace GGJ2025.Movement
{
    public class TransformLookAt : Updatable
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform TransformLooking;

        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Transform TransformTarget;

        protected override void Awake()
        {
            base.Awake();

            AddUpdate(deltTime => TransformLooking.LookAt(TransformTarget));
        }
    }
}
