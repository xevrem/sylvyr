using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AutomaticVerticalSize))]
public class AutoVerticalSizeEditor : Editor {

	public override void OnInspectorGUI () {

		DrawDefaultInspector ();

		if (GUILayout.Button ("Recalc Size")) {
			((AutomaticVerticalSize)target).adjust_size();
		}

		//base.OnInspectorGUI ();
	}

}
