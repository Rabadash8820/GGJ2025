using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "scene-activator", menuName = nameof(GGJ2025) + "/" + nameof(SceneActivator))]
    public class SceneActivator : ScriptableObject
    {
        public void SetActiveScene(string sceneName) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}
