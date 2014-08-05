using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Environment;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;
using TeamUtility.IO;

[CustomEditor(typeof(Dungeon))]
public class DungeonEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Dungeon Dungeon = (Dungeon)target;
		if(GUILayout.Button("Force Generate")) {
			Dungeon.Spawner.MinSpawns += 5;
			Dungeon.Spawner.MaxSpawns += Random.Range(5, 10);
			Dungeon.OutterGeneration();
		}
	}
}
