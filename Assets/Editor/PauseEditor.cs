using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.AI;
using Scurge.Audio;
using Scurge.Enemy;
using Scurge.Environment;
using Scurge.Networking;
using Scurge.Player;
using Scurge.Scoreboard;
using Scurge.UI;
using Scurge.Util;

[CustomEditor(typeof(Pause))]
public class PauseEditor : Editor {
	public override void OnInspectorGUI() {
		Pause Pause = (Pause)target;

		DrawDefaultInspector();
		if(GUILayout.Button("Default Controls")) {
			cInput.ResetInputs();
		}
	}
}
