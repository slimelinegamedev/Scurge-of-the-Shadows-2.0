using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
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
using TeamUtility.IO;
using System.Globalization;
using Holoville.HOEditorUtils;

public class LevelEditor : EditorWindow {

	public static EditorWindow window;
	public static Texture2D icon;

	public Vector2 mousePosition;
	public bool on;

	public GameObject movingObject;
	public LayerMask rayMask;
	public RaycastHit hit;
	public float rayDistance = 0;
	public Vector2 mousePositionAdder;
	public bool hasCasted = false;
	public GameObject selectedGameObject;
	public Vector3 pivotSet = Vector3.zero;

	public bool promptingAxis = false;
	public bool promptingScale = false;
	public Vector3 axisChoice;
	public Vector3 scaleChoice;
	public GameObject objectChoice;

	public bool showingDebug = false;
	public bool showingObjects = false;
	public int ObjectRemoveIndex;
	public List<placeableObject> objects;

	[MenuItem("Tools/Scurge/Dungeon Creation/Level Editor %#i")]
	static void Init() {
		window = EditorWindow.GetWindow(typeof(LevelEditor));
		icon = (Texture2D)Resources.Load("Level Editor Icon", typeof(Texture2D));
		HOPanelUtils.SetWindowTitle(window, icon, "Level Editor");
	}

	void OnEnable() {
		on = true;

		SceneView.onSceneGUIDelegate += SceneGUI;
		objects = LevelEditorData.instance.objects;
		movingObject = LevelEditorData.instance.movingObject;
		rayMask = LevelEditorData.instance.rayMask;
		pivotSet = LevelEditorData.instance.pivotSet;

		movingObject = (GameObject)Instantiate(movingObject);
	}
	void OnDestroy() {
		on = false;

		LevelEditorData.instance.objects = objects;
		LevelEditorData.instance.rayMask = rayMask;
		LevelEditorData.instance.pivotSet = pivotSet;
		DestroyImmediate(movingObject);
	}

	void Update() {

	}
	//TODO: Some scaling things are broken
	//TODO: Make it so you can move objects
	//TODO: Make it so you can go up on the Y
	void SceneGUI(SceneView sceneView) {
		if(on) {
			// This will have scene events including mouse down on scenes objects
			Event cur = Event.current;

			mousePosition = cur.mousePosition;
			mousePosition.x += mousePositionAdder.x;
			mousePosition.y += mousePositionAdder.y;
			//		mousePosition.y = mousePosition.y - Screen.height;
			//HandleUtility.GUIPointToWorldRay(cur.mousePosition)
			//Get the mouse ray ^
			if(movingObject != null) {
				Debug.DrawRay(Camera.current.transform.position + Camera.current.transform.forward * rayDistance, Camera.current.ScreenToWorldPoint(mousePosition), Color.cyan);

				if(Physics.Raycast(Camera.current.transform.position + Camera.current.transform.forward * 20, Camera.current.ScreenToWorldPoint(mousePosition), out hit, Mathf.Infinity, rayMask)) {
					Debug.Log("Hit Object Named " + hit.transform.gameObject.name);
					movingObject.transform.position = hit.transform.position;
					selectedGameObject = hit.transform.gameObject;
					hasCasted = true;
				}
			}
			DrawSceneGUI();
		}
	}
	void DrawSceneGUI() {
		Handles.BeginGUI();
		GUI.Label(new Rect(10, 10, 250, 50), "Scurge of the Shadows 2.0 Level Builder");

		GUI.Box(new Rect(10, Screen.height - 160, Screen.width - 20, 110), "Placeable Objects");

		int xPosition = 20;
		foreach(placeableObject curPlaceableObject in objects) {
			if(curPlaceableObject.obj != null && curPlaceableObject.name != "") {
				if(GUI.Button(new Rect(xPosition, Screen.height - 140, 64, 64), AssetPreview.GetAssetPreview(curPlaceableObject.obj))) {
					promptAxis();
					objectChoice = curPlaceableObject.obj;
				}
				GUI.Label(new Rect(xPosition, Screen.height - 70, 64, 20), curPlaceableObject.name);
				xPosition += 74;
			}
		}
		if(promptingAxis) {
			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 32, 64, 64), "Same")) {
				axisChoice = new Vector3(0, 0, 0);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}

			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 106, 64, 64), "Up")) {
				axisChoice = new Vector3(0, 1, 0);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - -42, 64, 64), "Down")) {
				axisChoice = new Vector3(0, -1, 0);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}

			if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 32, 64, 64), "Left")) {
				axisChoice = new Vector3(-1, 0, 0);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 32, 64, 64), "Right")) {
				axisChoice = new Vector3(1, 0, 0);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}

			if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 106, 64, 64), "Behind")) {
				axisChoice = new Vector3(0, 0, 1);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 106, 64, 64), "Forth")) {
				axisChoice = new Vector3(0, 0, -1);
				promptingAxis = false;
				Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.identity);
			}

			if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - -42, 64, 64), "Destroy")) {
				axisChoice = new Vector3(0, 0, 0);
				promptingAxis = false;
				DestroyImmediate(selectedGameObject);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - -42, 64, 64), "Scale")) {
				axisChoice = new Vector3(0, 0, 0);
				promptingAxis = false;
				promptingScale = true;
			}
		}
		if(promptingScale) {
			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 32, 64, 64), "Back")) {
				scaleChoice = new Vector3(0, 0, 0);
				promptingScale = false;
				promptingAxis = true;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0, selectedGameObject.transform.position.y + 0, selectedGameObject.transform.position.z + 0);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 106, 64, 64), "Up")) {
				scaleChoice = new Vector3(0, 1, 0);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0, selectedGameObject.transform.position.y + 0.5f, selectedGameObject.transform.position.z + 0);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - -42, 64, 64), "Down")) {
				scaleChoice = new Vector3(0, -1, 0);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0, selectedGameObject.transform.position.y + -0.5f, selectedGameObject.transform.position.z + 0);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 32, 64, 64), "Left")) {
				scaleChoice = new Vector3(-1, 0, 0);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + -0.5f, selectedGameObject.transform.position.y + 0, selectedGameObject.transform.position.z + 0);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 32, 64, 64), "Right")) {
				scaleChoice = new Vector3(1, 0, 0);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0.5f, selectedGameObject.transform.position.y + 0, selectedGameObject.transform.position.z + 0);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 106, 64, 64), "Behind")) {
				scaleChoice = new Vector3(0, 0, 1);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0, selectedGameObject.transform.position.y + 0, selectedGameObject.transform.position.z + 0.5f);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
			if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 106, 64, 64), "Forth")) {
				scaleChoice = new Vector3(0, 0, -1);
				promptingScale = false;
				selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + 0, selectedGameObject.transform.position.y + 0, selectedGameObject.transform.position.z + -0.5f);
				selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
			}
		}
		Handles.EndGUI();
	}
	void OnGUI() {
		movingObject = (GameObject)EditorGUILayout.ObjectField(movingObject, typeof(GameObject), true);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Distance Multiplier");
		rayDistance = EditorGUILayout.FloatField(rayDistance);
		GUILayout.EndHorizontal();
		pivotSet = EditorGUILayout.Vector3Field("Pivot", pivotSet);
		if(GUILayout.Button("Set Camera Pivot")) {
			SceneView.lastActiveSceneView.pivot = pivotSet;
		}

		if(movingObject != null && hasCasted) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Current Selected GameObject Is ");
			selectedGameObject = (GameObject)EditorGUILayout.ObjectField(selectedGameObject, typeof(GameObject), true);
			GUILayout.EndHorizontal();
		}
		else {
			GUILayout.Label("No GameObject Selected");
		}

		showingDebug = EditorGUILayout.Foldout(showingDebug, "Debug");
		if(showingDebug) {
			if(GUILayout.Button("Init Array")) {
				placeableObject initObject = new placeableObject();
				objects.Add(initObject);
			}
			rayMask = LayerMaskField("Raycast Layer Mask", rayMask, true);
		}

		showingObjects = EditorGUILayout.Foldout(showingObjects, "Placeable Object");
		if(showingObjects) {
			for(int iterateObjects = 0; iterateObjects < objects.Count; iterateObjects++) {
				objects [iterateObjects].name = EditorGUILayout.TextField("Name", objects [iterateObjects].name);
				objects [iterateObjects].obj = (GameObject)EditorGUILayout.ObjectField("Game Object", objects [iterateObjects].obj, typeof(GameObject), true);
			}
			if(GUILayout.Button("Add Object")) {
				objects.Add(null);
				ObjectRemoveIndex = objects.Count - 1;
			}
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Remove Object")) {
				objects.RemoveAt(ObjectRemoveIndex);
				ObjectRemoveIndex--;
			}
			ObjectRemoveIndex = EditorGUILayout.IntField(ObjectRemoveIndex);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Reset Objects")) {
				objects = new List<placeableObject>(0);
			}
			GUILayout.EndHorizontal();
		}
	}

	#region Utils

	public void promptAxis() {
		promptingAxis = true;
	}

	public void Slider(string label, float value, float min, float max) {
		GUILayout.BeginHorizontal();
		GUILayout.Label(label);
		mousePositionAdder.x = GUILayout.HorizontalSlider(value, min, max);
		GUILayout.Label(value.ToString());
		GUILayout.EndHorizontal();
	}

	public static List<string> layers;
	public static List<int> layerNumbers;
	public static string[] layerNames;
	public static long lastUpdateTick;

	/** Displays a LayerMask field.
  * \param showSpecial Use the Nothing and Everything selections
  * \param selected Current LayerMask
  * \version Unity 3.5 and up will use the EditorGUILayout.MaskField instead of a custom written one.
  */
	public static LayerMask LayerMaskField (string label, LayerMask selected, bool showSpecial) {

		//Unity 3.5 and up

		if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) {
			lastUpdateTick = System.DateTime.Now.Ticks;
			if (layers == null) {
				layers = new List<string>();
				layerNumbers = new List<int>();
				layerNames = new string[4];
			} else {
				layers.Clear ();
				layerNumbers.Clear ();
			}

			int emptyLayers = 0;
			for (int i=0;i<32;i++) {
				string layerName = LayerMask.LayerToName (i);

				if (layerName != "") {

					for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
					layerNumbers.Add (i);
					layers.Add (layerName);
				} else {
					emptyLayers++;
				}
			}

			if (layerNames.Length != layers.Count) {
				layerNames = new string[layers.Count];
			}
			for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
		}

		selected.value =  EditorGUILayout.MaskField (label,selected.value,layerNames);

		return selected;
	}
	#endregion
}
