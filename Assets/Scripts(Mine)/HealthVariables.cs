using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		public List<HeldItem> HeldItems;

		void Update() {
			gameObject.GetComponent<BoxCollider>().size += new Vector3(0, 0, Vector3.Distance(transform.position, Objects.Camera.transform.position));
		}

		void OnMouseOver() {
			if(EnemyStats.InRange) {
				EnemyStats.CanHit = true;
				ItemEnemyStats(HeldItems, HeldType.EnemyEnemyStats);
			}	
		}

		void OnMouseExit() {
			EnemyStats.CanHit = false;
			ItemEnemyStats(HeldItems, HeldType.Null);
		}

		public void ItemEnemyStats(List<HeldItem> curHeldItem, HeldType type) {
			if(type == HeldType.EnemyEnemyStats) {
				foreach(HeldItem curHeld in HeldItems) {
					curHeld.EnemyStats = EnemyStats.gameObject.GetComponent<EnemyStats>();
				}
			}
			else if(type == HeldType.Null) {
				foreach(HeldItem curHeld in HeldItems) {
					curHeld.EnemyStats = null;
				}
			}
		}
	}
}