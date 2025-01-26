using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2025.Editor
{
    /// <summary>
    /// Manages scene load order when Play Mode changes.
    /// </summary>
    /// <remarks>
    /// All scenes which the active scene depends on are opened before playing, then the original scene will be re-opened on stop.
    /// Based on an idea on this <a href="http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor">thread</a>
    /// and the <a href="https://wiki.unity3d.com/index.php/SceneAutoLoader">implementation</a> on this ancient wiki.
    /// </remarks>
    [InitializeOnLoad]
    public static class AutoSceneLoader
    {
        static AutoSceneLoader() => EditorApplication.playModeStateChanged += onPlayModeChanged;

        private static void onPlayModeChanged(PlayModeStateChange state)
        {
            // Cannot open scenes in the Editor while playing...
            if (EditorApplication.isPlaying)
                return;

            // Entering Play Mode, so load dependency scenes, if necessary
            Scene activeScene = SceneManager.GetActiveScene();
            if (EditorApplication.isPlayingOrWillChangePlaymode) {
                PreviousScenePath = activeScene.path;

                string[] beforeScenePaths = new[] {
                    "Assets/GGJ2025/scenes/bootstrap.unity",
                };

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                    Debug.Log($"Entering Play Mode in scene {activeScene.name}. First, additively loading scenes on which it depends...");
                    for (int s = 0; s < beforeScenePaths.Length; s++) {
                        string scenePath = beforeScenePaths[s];
                        Debug.Log($"Loading scene '{scenePath}'...");
                        try { _ = EditorSceneManager.OpenScene(scenePath, s == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive); }
                        catch (Exception ex) {
                            Debug.LogError($"Error loading scene '{scenePath}': {ex.Message}");
                            EditorApplication.isPlaying = false;
                            break;
                        }
                    }
                }
                else {  // User canceled saving
                    EditorApplication.isPlaying = false;
                    Debug.LogWarning($"Current modified scenes must be saved before load dependency scenes while entering Play Mode");
                }
            }

            // Exiting Play Mode, so reload previous scene
            else {
                try {
                    Debug.Log($"Exiting Play Mode and opening previous scene at '{PreviousScenePath}'...");
                    if (PreviousScenePath != activeScene.path)
                        _ = EditorSceneManager.OpenScene(PreviousScenePath);
                }
                catch (Exception ex) {
                    Debug.LogError($"Error opening previous scene at '{PreviousScenePath}': {ex.Message}");
                }
            }
        }

        private const string EDITOR_PREF_PREVIOUS_SCENE = nameof(AutoSceneLoader) + "." + nameof(PreviousScenePath);
        private static string PreviousScenePath {
            get => EditorPrefs.GetString(EDITOR_PREF_PREVIOUS_SCENE, SceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(EDITOR_PREF_PREVIOUS_SCENE, value);
        }
    }
}
