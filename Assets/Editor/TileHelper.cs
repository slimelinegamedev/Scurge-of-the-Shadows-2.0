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

public class TileHelper : EditorWindow {
	[MenuItem ("Utility/Tile Helper")]
	static void Init() {
		EditorWindow window = EditorWindow.GetWindow(typeof(TileHelper));
		window.title = "Tile Helper Utility";
	}
	public GameObject basicCube;
	public bool FloorMakerOpen = false;
	public Vector3 FirstFloorCoords;
	public Vector3 SecondFloorCoords;
	public int FloorY;

	void OnGUI() {
		basicCube = (GameObject)EditorGUILayout.ObjectField("Basic Cube", basicCube, typeof(GameObject), true);
		FloorMakerOpen = EditorGUILayout.Foldout(FloorMakerOpen, "Floor Creator");
		if(FloorMakerOpen) {
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			FloorY = EditorGUILayout.IntField("Floor Position Y", FloorY);
			GUILayout.EndHorizontal();
			GUILayout.Space(15);
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			FirstFloorCoords.x = EditorGUILayout.FloatField("Fill Area 1 X", FirstFloorCoords.x);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			FirstFloorCoords.z = EditorGUILayout.FloatField("Fill Area 1 Z", FirstFloorCoords.z);
			GUILayout.EndHorizontal();
			GUILayout.Space(15);
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			SecondFloorCoords.x = EditorGUILayout.FloatField("Fill Area 2 X", SecondFloorCoords.x);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			SecondFloorCoords.z = EditorGUILayout.FloatField("Fill Area 2 Z", SecondFloorCoords.z);
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Space(15);
			if(GUILayout.Button("Create Floor")) {
				if(EditorUtility.DisplayDialog("Are You Sure You Want To Create A Floor?", "This Cannot Be Undone", "Create", "Cancel")) {
					if(Selection.activeGameObject != null) {
						FillArea(FirstFloorCoords, SecondFloorCoords, Selection.activeGameObject);
					}
					else {
						EditorUtility.DisplayDialog("Action Failed", "Please Select A Hierarchy GameObject And Try Again", "Cancel", "Ok");
					}
				}
			}
			GUILayout.EndHorizontal();
		}
	}
	public void FillArea(Vector3 firstPoint, Vector3 secondPoint, GameObject parent) {
		var firstCube = (GameObject)Instantiate(basicCube, new Vector3(0, FloorY, 0), Quaternion.identity);
		firstCube.transform.parent = parent.transform;
		firstCube.transform.localPosition = new Vector3(0, FloorY, 0);
		for (int x = 0; x < firstPoint.x + secondPoint.x * 1; x += 1) {
		               	for (int z = 0; z < firstPoint.z + secondPoint.z; z += 1) {
		               		if(z > 0 || x > 0) {
			                	var CurBlock = (GameObject)Instantiate(basicCube, new Vector3(x, FloorY, z), Quaternion.identity);
			                	CurBlock.transform.parent = parent.transform;
			                	CurBlock.transform.localPosition = new Vector3(x, FloorY, z);
			                }
		                }
		}
	}
	void OnInspectorUpdate() {
		this.Repaint();
	}
}
