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
using Scurge.Scoreboard;

namespace Scurge.Util {
	public class EnemyCatcher : MonoBehaviour {
		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Enemy") {
				Destroy(collider.gameObject);
			}
		}
	}
}