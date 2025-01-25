using Microsoft.Extensions.Logging;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil;
using UnityUtil.DependencyInjection;
using UnityUtil.Logging;
using UnityUtil.Updating;

namespace GGJ2025
{
    public class Bootstrapper : MonoBehaviour
    {
        [RequiredIn(PrefabKind.NonPrefabInstance)]
        public Updater Updater;

        private void Awake()
        {
            var loggerFactory = new UnityDebugLoggerFactory();
            DependencyInjector.Instance.Initialize(loggerFactory);

            DependencyInjector.Instance.RegisterService<ILoggerFactory>(loggerFactory);
            DependencyInjector.Instance.RegisterService<IRuntimeIdProvider>(new RuntimeIdProvider());
            DependencyInjector.Instance.RegisterService<IUpdater>(Updater);
        }
    }
}
