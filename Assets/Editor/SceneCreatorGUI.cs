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

[CustomEditor(typeof(SceneCreator))]
public class SceneCreatorGUI : Editor {

	public static SceneCreator sceneCreator;
	public static bool ShowingGroups = false;
	public static bool IsShowingEnvironment = false;
	public static bool IsShowingEnemys = false;

	static SceneCreatorGUI() {
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}
	static void OnSceneGUI(SceneView sceneView) {
		DrawObjectsMenu();
	}
	public static void DrawObjectsMenu() {
		Handles.BeginGUI();

		float width = SceneView.lastActiveSceneView.position.width;
		float height = SceneView.lastActiveSceneView.position.height;

		if(GUI.Button(new Rect(10, 10, 100 , 25), "Create Object")) {
			Debug.Log("Opening Scene Creator Menu!");
			if(ShowingGroups) {
				ShowingGroups = false;
				IsShowingEnvironment = false;
				IsShowingEnemys = false;
			}
			if(!ShowingGroups) {
				ShowingGroups = true;
			}
		}
		if(ShowingGroups) {
			if(ShowingGroups) {
				if(GUI.Button(new Rect(10, 45, 100, 25), "Environment")) {
					IsShowingEnvironment = true;
					if(IsShowingEnemys) {
						IsShowingEnemys = false;
					}
				}
				if(GUI.Button(new Rect(10, 80, 100, 25), "Enemy")) {
					IsShowingEnemys = true;
					if(IsShowingEnvironment) {
						IsShowingEnvironment = false;
					}
				}
			}
		}
		if(IsShowingEnemys) {
			DisplayObjects(120, 45);
		}
		if(IsShowingEnvironment) {
			DisplayObjects(120, 45);
		}
		Handles.EndGUI();
	}
	public static void DisplayObjects(int x,  int y) {
		int posY = y;
		if(IsShowingEnemys) {
			posY += 35;
		}
		foreach(SceneObject curSceneObject in sceneCreator.objects) {
			if(IsShowingEnvironment && curSceneObject.type == SceneObjectType.Environment) {
				if(GUI.Button(new Rect(x, posY, 200, 25), curSceneObject.title)) {
					ShowingGroups = false;
					Debug.Log("Camera Position = " + SceneView.currentDrawingSceneView.camera.transform.position);
					Vector3 position = new Vector3(Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.x), Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.y), Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.z));
					var lastInstantiatedItem = (GameObject)Instantiate(curSceneObject.obj, position, Quaternion.identity);
					lastInstantiatedItem.SetActive(true);
					lastInstantiatedItem.transform.parent = Selection.activeGameObject.transform;
					Selection.activeGameObject = lastInstantiatedItem;
					IsShowingEnvironment = false;
					IsShowingEnemys = false;
				}
				posY += 35;
			}
			else if(IsShowingEnemys && curSceneObject.type == SceneObjectType.Enemy) {
				if(GUI.Button(new Rect(x, posY, 200, 25), curSceneObject.title)) {
					ShowingGroups = false;
					Debug.Log("Camera Position = " + SceneView.currentDrawingSceneView.camera.transform.position);
					Vector3 position = new Vector3(Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.x), Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.y), Mathf.Round(SceneView.currentDrawingSceneView.camera.transform.position.z));
					var lastInstantiatedItem = (GameObject)Instantiate(curSceneObject.obj, position, Quaternion.identity);
					lastInstantiatedItem.SetActive(true);
					lastInstantiatedItem.transform.parent = Selection.activeGameObject.transform;
					Selection.activeGameObject = lastInstantiatedItem;
					IsShowingEnvironment = false;
					IsShowingEnemys = false;
				}
				posY += 35;
			}
		}
	}
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		sceneCreator = (SceneCreator)target;
	}
}