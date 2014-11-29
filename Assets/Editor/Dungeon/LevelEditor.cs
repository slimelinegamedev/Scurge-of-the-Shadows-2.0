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
using Swing.Editor;

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
	public bool promptingScaleAmount = false;
	public int scaleAmount = 0;
	public int movingObjectYAdd = 0;
	public bool moving = false;
	public GameObject haveBlankObj;
	public Transform oldParentOfHaveBlankObj;
	public Vector3 axisChoice;
	public Vector3 scaleChoice;
	public GameObject objectChoice;

	public bool KeyCooldownPageDown = false;
	public bool KeyCooldownPageUp = false;

	public bool showingDebug = false;
	public bool showingObjects = false;
	public int ObjectRemoveIndex;
	public List<placeableObject> objects;
	public Vector3 spawnRotation;

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
	//TODO: Fix going up on the Y, it stops working after a while, and before that its too fast.
	void SceneGUI(SceneView sceneView) {
		if(on) {
			// This will have scene events including mouse down on scenes objects
			Event cur = Event.current;

			mousePosition = cur.mousePosition;
			mousePosition.x += mousePositionAdder.x;
			mousePosition.y += mousePositionAdder.y;
			//mousePosition.y = mousePosition.y - Screen.height;
			//HandleUtility.GUIPointToWorldRay(cur.mousePosition)
			//Get the mouse ray ^
			if(movingObject != null) {
				Debug.DrawRay(Camera.current.transform.position + Camera.current.transform.forward * rayDistance, Camera.current.ScreenToWorldPoint(mousePosition), Color.cyan);

				if(Physics.Raycast(Camera.current.transform.position + Camera.current.transform.forward * 20, Camera.current.ScreenToWorldPoint(mousePosition), out hit, Mathf.Infinity, rayMask)) {
					//Debug.Log("Hit Object Named " + hit.transform.gameObject.name);
					movingObject.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + movingObjectYAdd, hit.transform.position.z);
					movingObject.transform.localScale = hit.transform.localScale + new Vector3(0.2f, 0.2f, 0.2f);
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
		GUI.Label(new Rect(10, 30, 250, 50), "Keypad / To Move Object");
		GUI.Label(new Rect(10, 50, 250, 50), "Keypad * To Scale");
		GUI.Label(new Rect(10, 70, 250, 50), "Keypad . To Destroy");
		GUI.Label(new Rect(10, 90, 250, 50), "Page Up To Move Up");
		GUI.Label(new Rect(10, 110, 250, 50), "Page Down To Move Down");

		if(!moving) {
			GUI.Box(new Rect(10, Screen.height - 160, Screen.width - 20, 110), "Placeable Objects");

			int xPosition = 20;
			foreach(placeableObject curPlaceableObject in objects) {
				if(curPlaceableObject.obj != null && curPlaceableObject.name != "") {
					if(GUI.Button(new Rect(xPosition, Screen.height - 140, 64, 64), AssetPreview.GetAssetPreview(curPlaceableObject.obj))) {
						promptAxis();
						spawnRotation = curPlaceableObject.spawnRotation;
						objectChoice = curPlaceableObject.obj;
					}
					else if(Event.current.isKey && Event.current.keyCode == curPlaceableObject.combo){

					}
					GUI.Label(new Rect(xPosition, Screen.height - 70, 64, 20), curPlaceableObject.name);
					GUI.Label(new Rect(xPosition + 2, Screen.height - 140, 64, 64), curPlaceableObject.combo.ToString());
					xPosition += 74;
				}
			}
			if(promptingAxis) {
				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 32, 64, 64), "Same \n 5") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad5) {
					axisChoice = new Vector3(0, 0, 0);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}

				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 106, 64, 64), "Up \n 8") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad8) {
					axisChoice = new Vector3(0, 1, 0);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - -42, 64, 64), "Down \n 2") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad2) {
					axisChoice = new Vector3(0, -1, 0);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}

				if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 32, 64, 64), "Left \n 4") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad4) {
					axisChoice = new Vector3(-1, 0, 0);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}
				if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 32, 64, 64), "Right \n 6") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad6) {
					axisChoice = new Vector3(1, 0, 0);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}

				if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 106, 64, 64), "Behind \n 7") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad7) {
					axisChoice = new Vector3(0, 0, 1);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}
				if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 106, 64, 64), "Forth \n 9") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad9) {
					axisChoice = new Vector3(0, 0, -1);
					promptingAxis = false;
					Instantiate(objectChoice, selectedGameObject.transform.position + axisChoice, Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z));
				}
			}
			if(promptingScale) {
				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 32, 64, 64), "Same \n 5") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad5) {
					scaleChoice = new Vector3(0, 0, 0);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 106, 64, 64), "Up \n 8") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad8) {
					scaleChoice = new Vector3(0, 1, 0);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - -42, 64, 64), "Down \n 2") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad2) {
					scaleChoice = new Vector3(0, -1, 0);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 32, 64, 64), "Left \n 4") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad4) {
					scaleChoice = new Vector3(-1, 0, 0);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 32, 64, 64), "Right \n 6") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad6) {
					scaleChoice = new Vector3(1, 0, 0);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 106, Screen.height / 2 - 106, 64, 64), "Behind \n 7") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad7) {
					scaleChoice = new Vector3(0, 0, 1);
					promptingScale = false;
					promptingScaleAmount = true;
				}
				if(GUI.Button(new Rect(Screen.width / 2 - -42, Screen.height / 2 - 106, 64, 64), "Forth \n 9") || Event.current.isKey && Event.current.keyCode == KeyCode.Keypad9) {
					scaleChoice = new Vector3(0, 0, -1);
					promptingScale = false;
					promptingScaleAmount = true;
				}
			}
		}
		if(promptingScaleAmount) {
			if(GUI.Button(new Rect(Screen.width / 2 - 128, Screen.height / 2, 64, 64), "-")) {
				scaleAmount -= 1;
			}
			else if(Event.current.isKey && Event.current.keyCode == KeyCode.KeypadMinus) {
				scaleAmount -= 1;
			}
			if(GUI.Button(new Rect(Screen.width / 2 + 64, Screen.height / 2, 64, 64), "+")) {
				scaleAmount += 1;
			}
			else if(Event.current.isKey && Event.current.keyCode == KeyCode.KeypadPlus) {
				scaleAmount += 1;
			}
			if(GUI.Button(new Rect(Screen.width / 2 - 64, Screen.height / 2 + 64, 128, 64), "Done \n Keypad Enter") || Event.current.isKey && Event.current.keyCode == KeyCode.KeypadEnter) {
				promptingScaleAmount = false;
				if(scaleChoice.x > 0) {
					scaleChoice.x += scaleAmount;
					selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + scaleChoice.x / 2, selectedGameObject.transform.position.y + scaleChoice.y, selectedGameObject.transform.position.z + scaleChoice.z);
					selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
				}
				if(scaleChoice.y > 0) {
					scaleChoice.y += scaleAmount;
					selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + scaleChoice.x, selectedGameObject.transform.position.y + scaleChoice.y / 2, selectedGameObject.transform.position.z + scaleChoice.z);
					selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
				}
				if(scaleChoice.z > 0) {
					scaleChoice.z += scaleAmount;
					selectedGameObject.transform.position = new Vector3(selectedGameObject.transform.position.x + scaleChoice.x, selectedGameObject.transform.position.y + scaleChoice.y, selectedGameObject.transform.position.z + scaleChoice.z / 2);
					selectedGameObject.transform.localScale = new Vector3(selectedGameObject.transform.localScale.x + scaleChoice.x, selectedGameObject.transform.localScale.y + scaleChoice.y, selectedGameObject.transform.localScale.z + scaleChoice.z);
				}
				scaleAmount = 0;
			}
			GUI.Label(new Rect(Screen.width / 2 - 6, Screen.height / 2 + 24, 128, 64), scaleAmount.ToString());
		}
		if(Event.current.keyCode == KeyCode.PageUp && !KeyCooldownPageUp) {
			//Debug.Log("Moving Up!");
			movingObjectYAdd += 1;
			KeyCooldownPageUp = true;
			EditorCoroutine.start(CooldownPageUp());
//			EditorCoroutine.stop(CooldownPageUp());
		}
		if(Event.current.keyCode == KeyCode.PageDown && !KeyCooldownPageDown) {
			//Debug.Log("Moving Down!");
			movingObjectYAdd -= 1;
			KeyCooldownPageDown = true;
			EditorCoroutine.start(CooldownPageDown());
//			EditorCoroutine.stop(CooldownPageDown());
		}
		if(!promptingAxis && !promptingScale && !moving && !promptingScaleAmount) {
			if(Event.current.isKey) {
				if(Event.current.keyCode == KeyCode.KeypadDivide) {
					haveBlankObj = (GameObject)Instantiate(selectedGameObject, selectedGameObject.transform.position, Quaternion.identity);
					haveBlankObj.layer = 14;
					oldParentOfHaveBlankObj = selectedGameObject.transform.parent;
					haveBlankObj.transform.parent = selectedGameObject.transform.parent;
					haveBlankObj.renderer.material = LevelEditorData.instance.blankMat;

					selectedGameObject.transform.parent = movingObject.transform;
					selectedGameObject.transform.localPosition = Vector3.zero;
					selectedGameObject.layer = 0;

					moving = true;
				}
				if(Event.current.keyCode == KeyCode.KeypadMultiply) {
					promptingScale = true;
				}
				if(Event.current.keyCode == KeyCode.KeypadPeriod) {
					axisChoice = new Vector3(0, 0, 0);
					DestroyImmediate(selectedGameObject);
				}
			}
		}
		else if(moving) {
			//axisChoice = new Vector3(0, 0, 0);
			//promptingAxis = false;
			//promptingScale = true;
			//^ For Scale
			//
			//axisChoice = new Vector3(0, 0, 0);
			//promptingAxis = false;
			//DestroyImmediate(selectedGameObject);
			//^ For Destroy
			//
			//axisChoice = new Vector3(0, 0, 0);
			//promptingAxis = false;
			//moving = true;
			//
			//haveBlankObj = (GameObject)Instantiate(selectedGameObject, selectedGameObject.transform.position, Quaternion.identity);
			//oldParentOfHaveBlankObj = selectedGameObject.transform.parent;
			//haveBlankObj.transform.parent = selectedGameObject.transform.parent;
			//haveBlankObj.renderer.material = LevelEditorData.instance.blankMat;
			//
			//selectedGameObject.transform.parent = movingObject.transform;
			//selectedGameObject.transform.localPosition = Vector3.zero;
			//^ For Move

			if(GUI.Button(new Rect(Screen.width / 2 - 64, Screen.height - 114, 128, 64), "Place \n Keypad Enter")) {
				DestroyImmediate(haveBlankObj);
				foreach (Transform child in movingObject.transform) {
					if(child.name != "Point light") {
						child.parent = oldParentOfHaveBlankObj;
//						child.localScale = new Vector3(child.localScale.x + 0.1f, child.localScale.y + 0.1f, child.localScale.z + 0.1f);
					}
				}
				haveBlankObj.layer = 14;
				moving = false;
			}
			else if(Event.current.isKey && Event.current.keyCode == KeyCode.KeypadEnter) {
				DestroyImmediate(haveBlankObj);
				foreach (Transform child in movingObject.transform) {
					if(child.name != "Point light") {
						child.parent = oldParentOfHaveBlankObj;
						child.gameObject.layer = 14;
					}
				}
				moving = false;
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
	public IEnumerator CooldownPageUp() {
		while(true) {
			//Debug.Log("Starting To Wait...");
			yield return new WaitForSeconds(0.3f);
			//Debug.Log("ENOUGH WAITING!!!");
			KeyCooldownPageUp = false;
		}
	}
	public IEnumerator CooldownPageDown() {
		while(true) {
			//Debug.Log("Starting To Wait...");
			yield return new WaitForSeconds(0.3f);
			//Debug.Log("ENOUGH WAITING!!!");
			KeyCooldownPageDown = false;
		}
	}

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
