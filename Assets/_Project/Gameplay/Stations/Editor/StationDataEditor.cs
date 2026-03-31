using UnityEditor;

using UnityEngine;

namespace FPSCookingPrototype.Gameplay.Stations.Editor
{
	[CustomEditor(typeof(StationData))]
	public class StationDataEditor:UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawDefaultInspector();

			StationData data = (StationData)target;

			if( GUILayout.Button("Validate") )
			{
				data.Validate();
				EditorUtility.SetDirty(data);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}