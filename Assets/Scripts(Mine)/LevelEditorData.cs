using UnityEngine;
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

[System.Serializable]
public class placeableObject {
	public string name;
	public GameObject obj;
	public Vector3 spawnRotation;
	public KeyCode combo;
}

[ExecuteInEditMode]
public class LevelEditorData : MonoBehaviour {

	public static LevelEditorData instance;

	public List<placeableObject> objects;
	public GameObject movingObject;
	public LayerMask rayMask;
	public Vector3 pivotSet;
	public Material blankMat;

	void OnEnable() {
		instance = gameObject.GetComponent<LevelEditorData>();
	}
}
