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

namespace Scurge.Environment {
	public class NextFloor : MonoBehaviour {
		public Dungeon Dungeon;

		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				Dungeon.OutterGeneration();
			}
		}
	}
}