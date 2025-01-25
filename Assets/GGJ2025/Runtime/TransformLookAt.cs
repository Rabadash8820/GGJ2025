using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.Updating;

namespace GGj2025
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
