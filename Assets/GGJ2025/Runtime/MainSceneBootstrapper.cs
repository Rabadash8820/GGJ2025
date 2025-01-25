using Microsoft.Extensions.Logging;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtil.DependencyInjection;
using UnityUtil.Logging;
using UnityUtil.Updating;

public class MainSceneBootstrapper : MonoBehaviour
{
    [RequiredIn(PrefabKind.NonPrefabInstance)]
    public Updater Updater;

    private void Awake()
    {
        var loggerFactory = new UnityDebugLoggerFactory();
        DependencyInjector.Instance.Initialize(loggerFactory);

        DependencyInjector.Instance.RegisterService<ILoggerFactory>(loggerFactory);
    }
}
