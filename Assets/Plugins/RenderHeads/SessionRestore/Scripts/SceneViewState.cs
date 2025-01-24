#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//-----------------------------------------------------------------------------
// Copyright 2017-2022 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.SessionRestore
{
	internal static class SceneViewState
	{
		internal static void RestoreState(string sceneId)
		{
			SceneView sv = SceneView.lastActiveSceneView;
			if (sv)
			{
				try
				{
					#if UNITY_2018_2_OR_NEWER
					DrawCameraMode drawCameraMode = (DrawCameraMode)Load("SceneView.CameraMode", sceneId, (int)DrawCameraMode.Normal);
					sv.cameraMode = SceneView.GetBuiltinCameraMode(drawCameraMode);
					#else
					sv.renderMode = (DrawCameraMode)Load("SceneView.RenderMode", sceneId, (int)sv.renderMode);
					#endif
				}
				catch (System.InvalidOperationException)
				{
					// Silently catch this exception that has been reported in Unity 2021.3.2f1
					// Presumably because a DrawCameraMode enum index changed or became invalid.
				}

				#if UNITY_2019_3_OR_NEWER
				sv.showGrid = Load("SceneView.ShowGrid", sceneId, sv.showGrid);
				#endif

				sv.in2DMode = Load("SceneView.In2DMode", sceneId, sv.in2DMode);
				sv.orthographic = Load("SceneView.Orthographic", sceneId, sv.orthographic);
				sv.size = Load("SceneView.Size", sceneId, sv.size);
				sv.pivot = Load("SceneView.Pivot", sceneId, sv.pivot);
				if (!sv.in2DMode)
				{
					sv.rotation = Load("SceneView.Rotation", sceneId, sv.rotation);
				}
				
				if (sv.camera)
				{
					sv.camera.fieldOfView = Load("SceneView.Camera.FOV", sceneId, sv.camera.fieldOfView);
					sv.camera.transform.rotation = sv.rotation;

					#if UNITY_2019_1_OR_NEWER
					float cameraDistance = Load("SceneView.CameraDistance", sceneId, sv.cameraDistance);
					sv.camera.transform.position = sv.pivot - sv.camera.transform.rotation * new Vector3(0f, 0f, -cameraDistance);
					#else
					sv.camera.transform.position = Load("SceneView.Camera.Position", sceneId, sv.camera.transform.position);
					#endif
				}
			}
		}
		internal static void SaveState(string sceneId)
		{
			SceneView sv = SceneView.lastActiveSceneView;
			if (sv)
			{
				#if UNITY_2018_2_OR_NEWER
				Save("SceneView.CameraMode", sceneId, (int)sv.cameraMode.drawMode);
				#else
				Save("SceneView.RenderMode", sceneId, (int)sv.renderMode);
				#endif

				#if UNITY_2019_3_OR_NEWER
				Save("SceneView.ShowGrid", sceneId, sv.showGrid);
				#endif

				Save("SceneView.In2DMode", sceneId, sv.in2DMode);
				Save("SceneView.Orthographic", sceneId, sv.orthographic);
				Save("SceneView.Size", sceneId, sv.size);
				Save("SceneView.Pivot", sceneId, sv.pivot);
				Save("SceneView.Rotation", sceneId, sv.rotation);
				#if UNITY_2019_1_OR_NEWER
				Save("SceneView.CameraDistance", sceneId, sv.cameraDistance);
				#endif
				if (sv.camera)
				{
					Save("SceneView.Camera.FOV", sceneId, sv.camera.fieldOfView);
					Save("SceneView.Camera.Position", sceneId, sv.camera.transform.position);
				}
			}
		}

		private static void Save(string name, string sceneId, bool p)
		{
			name = GetPrefName(name, sceneId);
			EditorPrefs.SetBool(name, p);
		}

		private static bool Load(string name, string sceneId, bool p)
		{
			name = GetPrefName(name, sceneId);
			p = EditorPrefs.GetBool(name, p);
			return p;
		}

		private static void Save(string name, string sceneId, int p)
		{
			name = GetPrefName(name, sceneId);
			EditorPrefs.SetInt(name, p);
		}

		private static int Load(string name, string sceneId, int p)
		{
			name = GetPrefName(name, sceneId);
			p = EditorPrefs.GetInt(name, p);
			return p;
		}		

		private static void Save(string name, string sceneId, float p)
		{
			name = GetPrefName(name, sceneId);
			EditorPrefs.SetFloat(name, p);
		}

		private static float Load(string name, string sceneId, float p)
		{
			name = GetPrefName(name, sceneId);
			p = EditorPrefs.GetFloat(name, p);
			return p;
		}

		private static void Save(string name, string sceneId, Vector3 p)
		{
			name = GetPrefName(name, sceneId);
			EditorPrefs.SetFloat(name + ".X", p.x);
			EditorPrefs.SetFloat(name + ".Y", p.y);
			EditorPrefs.SetFloat(name + ".Z", p.z);
		}

		private static Vector3 Load(string name, string sceneId, Vector3 p)
		{
			name = GetPrefName(name, sceneId);
			p.x = EditorPrefs.GetFloat(name + ".X", p.x);
			p.y = EditorPrefs.GetFloat(name + ".Y", p.y);
			p.z = EditorPrefs.GetFloat(name + ".Z", p.z);
			return p;
		}

		private static void Save(string name, string sceneId, Quaternion p)
		{
			name = GetPrefName(name, sceneId);
			EditorPrefs.SetFloat(name + ".X", p.x);
			EditorPrefs.SetFloat(name + ".Y", p.y);
			EditorPrefs.SetFloat(name + ".Z", p.z);
			EditorPrefs.SetFloat(name + ".W", p.w);
		}

		private static Quaternion Load(string name, string sceneId, Quaternion p)
		{
			name = GetPrefName(name, sceneId);
			p.x = EditorPrefs.GetFloat(name + ".X", p.x);
			p.y = EditorPrefs.GetFloat(name + ".Y", p.y);
			p.z = EditorPrefs.GetFloat(name + ".Z", p.z);
			p.w = EditorPrefs.GetFloat(name + ".W", p.w);
			return p;
		}

		private static string GetPrefName(string name, string sceneId)
		{
			return string.Format("RenderHeads-HierarchyRestore-{0}-{1}", name, sceneId);
		}
	}
}
#endif