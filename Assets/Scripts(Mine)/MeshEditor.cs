using UnityEngine;
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

[ExecuteInEditMode]

public class MeshEditor : MonoBehaviour {

	public Mesh mesh;
	public GameObject handleMesh;
	public Vector3[] verts;
	public Vector3 vertPos;
	public GameObject[] handles;
	
	void OnEnable() {
		handleMesh = GameObject.Find("Handle Mesh");
		mesh = GetComponent<MeshFilter>().sharedMesh;
		verts = mesh.vertices;
		foreach(Vector3 vert in verts) {
			vertPos = transform.TransformPoint(vert);
			GameObject handle = new GameObject("handle");
			handle.transform.position = vertPos;
			handle.transform.parent = transform;
			handle.tag = "handle";
			var lastHandleObject = (GameObject)Instantiate(handleMesh, handle.transform.position, Quaternion.identity);
			lastHandleObject.SetActive(true);
			lastHandleObject.transform.parent = handle.transform;
			//handle.AddComponent<Gizmo_Sphere>();
			print(vertPos);
		}
	}
	
	void OnDisable() {
		GameObject[] handles = GameObject.FindGameObjectsWithTag("handle");
		foreach(GameObject handle in handles) {
			DestroyImmediate(handle);   
		}
	}
	
	void Update() {
		handles = GameObject.FindGameObjectsWithTag("handle");
		for(int i = 0; i < verts.Length; i++) {
			verts [i] = handles [i].transform.localPosition;  
		}
		mesh.vertices = verts;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}