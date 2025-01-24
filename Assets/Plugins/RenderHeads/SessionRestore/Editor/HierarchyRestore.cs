#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using RenderHeads.SessionRestore;

//-----------------------------------------------------------------------------
// Copyright 2017-2022 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.SessionRestore.Editor
{
	/// <summary>
	/// Create a hidden node in the scene which tracks the selection state and hierarchy expansion state
	/// When this node is deleted (by changing scene) it will save the last state it has.
	/// When the node is created it will try to restore the selection and hierarchy expansion state if it
	/// has this saved to EditorPrefs for the specific project and scene.
	/// </summary>
	[InitializeOnLoad]
	public partial class HierarchyRestoreEditor
	{
		private const string GameObjectName = "Temp-SceneCreatedMarker";
		private static GameObject _cachedGo = null;
		private static HierarchyNode _cachedNode = null;
		
		// This is called when Unity first loads and also each time any scripts are compiled
		static HierarchyRestoreEditor()
		{
			RefreshEnabled();
		}

		internal static void RefreshEnabled()
		{
			// The hierarchyWindowChanged fires when nodes are added, moved or removed from the hierarchy
			// Loading a new scene also changes the hierarchy
			// NOTE: This doesn't get called when nodes are expanded/collapsed
#if UNITY_2018_1_OR_NEWER
			EditorApplication.hierarchyChanged -= OnHierarchyWindowChanged;
			if (HierarchyRestore.IsEnabled)
			{
				EditorApplication.hierarchyChanged += OnHierarchyWindowChanged;
			}
#else
			EditorApplication.hierarchyWindowChanged -= OnHierarchyWindowChanged;
			if (HierarchyRestore.IsEnabled)
			{
				EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
			}
#endif
		}

		private static void CreateSceneNode()
		{
			// Create our marker
			_cachedGo = new GameObject(GameObjectName, typeof(HierarchyNode));
			_cachedNode = _cachedGo.GetComponent<HierarchyNode>();
			SetSceneNodeFlags();
		}

		internal static void SetSceneNodeFlags()
		{
			if (_cachedGo != null)
			{
				// Note: We don't use the DontSaveInEditor flag as this causes OnDestroy/OnDisable() to not be called when the scene unloads
				_cachedGo.hideFlags = HideFlags.NotEditable | HideFlags.HideInHierarchy | HideFlags.HideInInspector;

#if UNITY_5 || UNITY_5_4_OR_NEWER
				_cachedGo.hideFlags |= HideFlags.DontSaveInBuild;
#endif

				// Toggle node visibility
				if (HierarchyRestore.ShowNode)
				{
					_cachedGo.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
				}
			}
		}

		private static void OnHierarchyWindowChanged()
		{
			if (HierarchyUtils.IsHierarchyWindowInPrefabMode())
			{
				return;
			}

			if (IsSceneNodeRequired())
			{
				CreateSceneNode();
			}

			if (_cachedNode != null)
			{
				_cachedNode.SetHierarchyDirty();
			}
		}

		internal static bool IsSceneNodeRequired()
		{
			bool result = false;
			if (_cachedGo == null && !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isUpdating)
			{
				_cachedGo = GameObject.Find(GameObjectName);
				// It's a new scene because our marker node doesn't exist
				if (_cachedGo == null)
				{
					result = true;
				}
				else
				{
					_cachedNode = _cachedGo.GetComponent<HierarchyNode>();
				}
			}
			return result;
		}
	}
}
#endif