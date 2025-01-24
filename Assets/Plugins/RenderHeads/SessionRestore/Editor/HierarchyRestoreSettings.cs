#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//-----------------------------------------------------------------------------
// Copyright 2017-2022 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.SessionRestore.Editor
{
	public partial class HierarchyRestoreEditor
	{
#if UNITY_2018_3_OR_NEWER
		private class MyPrefSettingsProvider : SettingsProvider
		{
			public MyPrefSettingsProvider(string path, SettingsScope scopes = SettingsScope.User)
			: base(path, scopes)
			{ }

			public override void OnGUI(string searchContext)
			{
				CustomPreferencesGUI();
			}
		}

		[SettingsProvider]
		static SettingsProvider MyNewPrefCode()
		{
			return new MyPrefSettingsProvider("Preferences/Session Restore");
		}

#elif UNITY_5_6_OR_NEWER
		[PreferenceItem("Session Restore")]
#endif
		private static void CustomPreferencesGUI()
		{
			{
				EditorGUI.BeginChangeCheck();
				bool isEnabled = EditorGUILayout.Toggle("Globally Enabled: ", HierarchyRestore.IsEnabled);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.IsEnabled = isEnabled;
					HierarchyRestoreEditor.RefreshEnabled();
				}
			}
			{
				EditorGUI.BeginChangeCheck();
				bool isRestoreSceneView = EditorGUILayout.Toggle("Restore SceneView: ", HierarchyRestore.RestoreSceneView);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.RestoreSceneView = isRestoreSceneView;
				}
			}
			{
				EditorGUI.BeginChangeCheck();
				EnterExitPlayMode enterPlayMode = (EnterExitPlayMode)EditorGUILayout.EnumPopup("Enter Play Mode: ", HierarchyRestore.EnterPlayMode);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.EnterPlayMode = enterPlayMode;
				}
			}
			{
				EditorGUI.BeginChangeCheck();
				EnterExitPlayMode exitPlayMode = (EnterExitPlayMode)EditorGUILayout.EnumPopup("Exit Play Mode: ", HierarchyRestore.ExitPlayMode);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.ExitPlayMode = exitPlayMode;
				}
			}			
			EditorGUILayout.Space();
			GUILayout.Label("Debugging", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;

			{
				EditorGUI.BeginChangeCheck();
				bool isDebugLog = EditorGUILayout.Toggle("Debug Log: ", HierarchyRestore.DebugLog);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.DebugLog = isDebugLog;
				}
			}	
			{
				EditorGUI.BeginChangeCheck();
				bool isShowNode = EditorGUILayout.Toggle("Show Node: ", HierarchyRestore.ShowNode);
				if (EditorGUI.EndChangeCheck())
				{
					HierarchyRestore.ShowNode = isShowNode;
					HierarchyRestoreEditor.IsSceneNodeRequired();
					HierarchyRestoreEditor.SetSceneNodeFlags();
					
					// The hierarchy window must be refreshed
					EditorApplication.DirtyHierarchyWindowSorting();
				}
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.BeginHorizontal();
			// NOTE: GUILayout.Button() doesn't follow EditorGUI.indentLevel so we manually indent
			GUILayout.Space(16f);
			if (GUILayout.Button("Save Scene Session", GUILayout.ExpandWidth(false)))
			{
				HierarchyNode node = HierarchyNode.GetNode();
				if (node)
				{
					node.SaveState();
				}
			}
			if (GUILayout.Button("Restore Scene Session", GUILayout.ExpandWidth(false)))
			{
				HierarchyNode node = HierarchyNode.GetNode();
				if (node)
				{
					node.RestoreState();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif