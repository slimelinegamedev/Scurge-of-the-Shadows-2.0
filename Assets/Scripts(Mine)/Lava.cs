using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;

namespace Scurge.Environment {
	public class Lava : MonoBehaviour {

		public Stats Stats;
		public EnemyStats EnemyStats;

		public bool PlayerIn = false;
		public bool EnemyIn = false;

		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				PlayerIn = true;
			}
			else if(collider.gameObject.tag == "Enemy") {
				//Broken. Let them take a lava bath for now
				//EnemyStats = collider.gameObject.transform.Find("Health").GetComponent<EnemyStats>();
				//EnemyIn = true;
			}
		}
		void OnTriggerExit(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				PlayerIn = false;
			}
			else if(collider.gameObject.tag == "Enemy") {
				EnemyIn = false;
			}	
		}

		void Update() {
			Hurt();
		}
		public void Hurt() {
			if(PlayerIn) {
				Stats.Hurt(Random.Range(1, 3));
			}
			if(EnemyIn) {
				EnemyStats.DealDamage(Random.Range(1, 3));
			}
		}
	}
}