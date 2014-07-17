using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Environment;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombine : MonoBehaviour {

	public bool AddCollider = true;
	public Material mat;

	void Start() {
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.active = false;
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false);
		transform.gameObject.active = true;
		if(AddCollider) {
			transform.gameObject.AddComponent<MeshCollider>();
		}
		transform.localScale = new Vector3(1.619f, 1.619f, 1.619f);
		renderer.material = mat;
	}
}