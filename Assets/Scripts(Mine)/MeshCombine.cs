﻿using UnityEngine;
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

	public bool generateTriangleStrips = true;
	public bool castShadows = true;
	public bool receiveShadows = true;
	public int wait;
	public bool addCollider;
	public Material mat;

	void Start() {
		CombineMesh(gameObject, wait);
	}

	public IEnumerator CombineMesh(GameObject obj, int Timer) {
		yield return new WaitForSeconds(Timer);
		Component[] filters  = obj.GetComponentsInChildren(typeof(MeshFilter));
		Matrix4x4 myTransform = obj.transform.worldToLocalMatrix;
		Hashtable materialToMesh= new Hashtable();

		for (int i=0;i<filters.Length;i++) {
			MeshFilter filter = (MeshFilter)filters[i];
			Renderer curRenderer  = filters[i].GetComponent<Renderer>();
			MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance ();
			instance.mesh = filter.sharedMesh;
			if (curRenderer != null && curRenderer.enabled && instance.mesh != null) {
				instance.transform = myTransform * filter.transform.localToWorldMatrix;

				Material[] materials = curRenderer.sharedMaterials;
				for (int m=0;m<materials.Length;m++) {
					instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);

					ArrayList objects = (ArrayList)materialToMesh[materials[m]];
					if (objects != null) {
						objects.Add(instance);
					}
					else
					{
						objects = new ArrayList ();
						objects.Add(instance);
						materialToMesh.Add(materials[m], objects);
					}
				}

				curRenderer.enabled = false;
			}
		}

		foreach (DictionaryEntry de  in materialToMesh) {
			ArrayList elements = (ArrayList)de.Value;
			MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));

			// We have a maximum of one material, so just attach the mesh to our own game object
			if (materialToMesh.Count == 1)
			{
				// Make sure we have a mesh filter & renderer
				if (obj.GetComponent(typeof(MeshFilter)) == null)
					obj.AddComponent(typeof(MeshFilter));
				if (!obj.GetComponent("MeshRenderer"))
					obj.AddComponent<MeshRenderer>();

				MeshFilter filter = (MeshFilter)obj.GetComponent(typeof(MeshFilter));
				filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
				obj.GetComponent<Renderer>().material = (Material)de.Key;
				obj.GetComponent<Renderer>().enabled = true;
				obj.GetComponent<Renderer>().castShadows = castShadows;
				obj.GetComponent<Renderer>().receiveShadows = receiveShadows;
				obj.AddComponent<MeshCollider>();
				obj.GetComponent<Renderer>().material = mat;
				if(addCollider) {
					obj.AddComponent<MeshCollider>();
				}
			}
			// We have multiple materials to take care of, build one mesh / gameobject for each material
			// and parent it to this object
			else
			{
				GameObject go = new GameObject("Combined mesh");
				go.transform.parent = obj.transform;
				go.transform.localScale = Vector3.one;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localPosition = Vector3.zero;
				go.AddComponent(typeof(MeshFilter));
				go.AddComponent<MeshRenderer>();
				go.GetComponent<Renderer>().material = (Material)de.Key;
				MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
				filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
				go.AddComponent<MeshCollider>();
			}
		}
	}
}