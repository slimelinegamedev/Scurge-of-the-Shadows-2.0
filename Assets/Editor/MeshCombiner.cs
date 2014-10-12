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

namespace Scurge.Editor {
	public class MeshCombiner : EditorWindow {
		[MenuItem ("Tools/Scurge/Utility/Mesh Combiner")]
		static void Init () {
			EditorWindow window = EditorWindow.GetWindow(typeof (MeshCombiner));
			window.title = "Mesh Combiner Utility";
		}

		public bool generateTriangleStrips = true;
		public bool castShadows = true;
		public bool receiveShadows = true;
		public bool keepOld = true;

		public List<GameObject> oldMeshPieces;
		public GameObject oldMeshHolder;

		void OnGUI() {
			generateTriangleStrips = EditorGUILayout.Toggle("Generate Triangle Strips", generateTriangleStrips);
			castShadows = EditorGUILayout.Toggle("Cast Shadows", castShadows);
			receiveShadows = EditorGUILayout.Toggle("Receive Shadows", receiveShadows);
			keepOld = EditorGUILayout.Toggle("Keep Old Mesh Parts", keepOld);
			if(GUILayout.Button("Combine")) {
				if(EditorUtility.DisplayDialog("Are You Sure You Want To Combine These Meshes?", "This Cannot Be Undone", "Combine", "Stop")) {
					CombineMesh(Selection.activeGameObject);
				}
			}
		}
		public void CombineMesh(GameObject obj) {
			Component[] filters  = obj.GetComponentsInChildren(typeof(MeshFilter));
			Matrix4x4 myTransform = obj.transform.worldToLocalMatrix;
			Hashtable materialToMesh= new Hashtable();

			if(keepOld) {
				foreach(Component curMeshPiece in filters) {
					oldMeshPieces.Add(curMeshPiece.gameObject);
				}
				oldMeshHolder = new GameObject("Old Mesh");
				oldMeshHolder.transform.parent = obj.transform;
			}
			if(keepOld) {
				for(int iterateOldMeshPieces = 0; iterateOldMeshPieces < oldMeshPieces.Count; iterateOldMeshPieces++) {
					oldMeshPieces[iterateOldMeshPieces].transform.parent = oldMeshHolder.transform;
					oldMeshPieces.RemoveAt(iterateOldMeshPieces);
				}
			}

			for (int i=0;i<filters.Length;i++) {
				MeshFilter filter = (MeshFilter)filters[i];
				Renderer curRenderer  = filters[i].renderer;
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
						obj.AddComponent("MeshRenderer");

					MeshFilter filter = (MeshFilter)obj.GetComponent(typeof(MeshFilter));
					filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
					obj.renderer.material = (Material)de.Key;
					obj.renderer.enabled = true;
					obj.renderer.castShadows = castShadows;
					obj.renderer.receiveShadows = receiveShadows;
					obj.AddComponent<MeshCollider>();
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
					go.AddComponent("MeshRenderer");
					go.renderer.material = (Material)de.Key;
					MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
					filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
					go.AddComponent<MeshCollider>();
				}
			}
		}
	}
}