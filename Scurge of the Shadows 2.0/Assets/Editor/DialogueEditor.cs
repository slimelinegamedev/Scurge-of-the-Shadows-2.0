using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;
using Scurge.Environment;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor {
	public override void OnInspectorGUI() {
		Dialogue dialogue = (Dialogue)target;
		DrawDefaultInspector();
	}
}