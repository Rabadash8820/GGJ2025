#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//-----------------------------------------------------------------------------
// Copyright 2017-2022 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.SessionRestore
{
	public enum EnterExitPlayMode
	{
		DoNothing,
		RestoreSession,
	}

	public class HierarchyRestore
	{
		// Note that "//" is used to separate path items in the list because Unity can not use "//" in a node name
		internal static string PathListSeparator = System.Convert.ToChar(177).ToString();

		private const string SettingsPrefix = "SessionRestore.";
		public static bool IsEnabled
		{
			get
			{
				return EditorPrefs.GetBool(SettingsPrefix + "Enable", true);
			}
		 	set
			{
				EditorPrefs.SetBool(SettingsPrefix + "Enable", value);
			}
		}
		public static EnterExitPlayMode EnterPlayMode
		{
			get
			{
				return (EnterExitPlayMode)EditorPrefs.GetInt(SettingsPrefix + "EnterPlayMode", (int)EnterExitPlayMode.DoNothing);
			}
		 	set
			{
				EditorPrefs.SetInt(SettingsPrefix + "EnterPlayMode", (int)value);
			}
		}
		public static EnterExitPlayMode ExitPlayMode
		{
			get
			{
				return (EnterExitPlayMode)EditorPrefs.GetInt(SettingsPrefix + "ExitPlayMode", (int)EnterExitPlayMode.DoNothing);
			}
		 	set
			{
				EditorPrefs.SetInt(SettingsPrefix + "ExitPlayMode", (int)value);
			}
		}
		public static bool RestoreSceneView
		{
			get
			{
				return EditorPrefs.GetBool(SettingsPrefix + "RestoreSceneView", true);
			}
		 	set
			{
				EditorPrefs.SetBool(SettingsPrefix + "RestoreSceneView", value);
			}
		}		
		public static bool DebugLog
		{
			get
			{
				return EditorPrefs.GetBool(SettingsPrefix + "DebugLog", false);
			}
		 	set
			{
				EditorPrefs.SetBool(SettingsPrefix + "DebugLog", value);
			}
		}
		public static bool ShowNode
		{
			get
			{
				return EditorPrefs.GetBool(SettingsPrefix + "ShowNode", false);
			}
		 	set
			{
				EditorPrefs.SetBool(SettingsPrefix + "ShowNode", value);
			}
		}
	}
}
#endif