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

namespace Scurge.Environment {
	public class DungeonPropSpot : MonoBehaviour {

		#region vars
		public Dungeon Dungeon;
		public PropSize size;
		#endregion

		void OnEnable() {
			StartCoroutine(PlaceProp());
		}
		IEnumerator PlaceProp() {
			DungeonProp prop = Dungeon.Props[Random.Range(0, Dungeon.Props.Count)];
			while(prop.size != size) {
				prop = Dungeon.Props[Random.Range(0, Dungeon.Props.Count)];
				yield return new WaitForSeconds(0.1f);
			}
			if(prop.size == size) {
				var lastClonedProp = (GameObject)Instantiate(prop.gameObject, transform.position, Quaternion.identity);
			}
		}
	}
}