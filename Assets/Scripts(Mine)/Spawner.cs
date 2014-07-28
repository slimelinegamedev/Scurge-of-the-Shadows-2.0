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


namespace Scurge.Enemy {
	public class Spawner : MonoBehaviour {
		public List<GameObject> Enemys;
		public GameObject LastEnemy;
		public Vector3 SpawnPosition;

		public GameObject Holder;

		public List<GameObject> SpawnedEnemys;
		public List<int> PosibleYPositions;

		public int MinSpawns;
		public int MaxSpawns;
		public int Spawned;

		public void Spawn() {
			if(SpawnedEnemys.Count > 0) {
				foreach (GameObject curEnemyDestroying in SpawnedEnemys) {
					Destroy(curEnemyDestroying);
				}
			}
			SpawnedEnemys = new List<GameObject>(0);
			for(Spawned = 0; Spawned < Random.Range(MinSpawns, MaxSpawns); Spawned++) {
				LastEnemy = Enemys[Random.Range(0, Enemys.Count)];
				SpawnPosition = new Vector3(Random.Range(-20, 90), PosibleYPositions[Random.Range(0, PosibleYPositions.Count)], Random.Range(-20, 85));
				var LastEnemySpawned = (GameObject)Object.Instantiate(LastEnemy, SpawnPosition, transform.rotation);
				LastEnemySpawned.SetActive(true);
				SpawnedEnemys.Add(LastEnemySpawned);
				LastEnemySpawned.transform.parent = Holder.transform;
			}
		}
 	}
}