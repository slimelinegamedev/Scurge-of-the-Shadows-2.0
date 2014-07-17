using UnityEngine;
using System.Collections;
using Scurge.Util;
using Scurge.Player;
using Scurge.Enemy;

namespace Scurge.Enemy {

	public enum HeldType {
		Null,
		EnemyEnemyStats
	}

	public class HealthVariables : MonoBehaviour {

		public EnemyStats EnemyStats;
		public Objects Objects;
		public HeldItem[] HeldItem;

		void Update() {
			gameObject.GetComponent<BoxCollider>().size += new Vector3(0, 0, Vector3.Distance(transform.position, Objects.Camera.transform.position));
		}

		void OnMouseOver() {
			if(EnemyStats.InRange) {
				EnemyStats.CanHit = true;
				ItemEnemyStats(HeldItem, HeldType.EnemyEnemyStats);
			}	
		}
		void OnMouseExit() {
			EnemyStats.CanHit = false;
			ItemEnemyStats(HeldItem, HeldType.Null);
		}


		public void ItemEnemyStats(HeldItem[] curHeldItem, HeldType type) {
			if(type == HeldType.EnemyEnemyStats) {
				foreach(HeldItem curHeld in HeldItem) {
					curHeld.EnemyStats = EnemyStats.gameObject.GetComponent<EnemyStats>();
				}
			}
			else if(type == HeldType.Null) {
				foreach(HeldItem curHeld in HeldItem) {
					curHeld.EnemyStats = null;
				}
			}
		}
	}
}