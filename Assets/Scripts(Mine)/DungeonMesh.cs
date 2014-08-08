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
using TeamUtility.IO;

namespace Scurge.Environment {
	public class DungeonMesh : MonoBehaviour {

		public GameObject Holder;
		public int WaitTime;
		public MeshCombine MeshCombine;

		public IEnumerator Start() {
			yield return new WaitForSeconds(WaitTime);
			gameObject.transform.parent = Holder.transform;
			MeshCombine.CombineMesh(MeshCombine.transform.gameObject, MeshCombine.wait);
		}
		
	}
}