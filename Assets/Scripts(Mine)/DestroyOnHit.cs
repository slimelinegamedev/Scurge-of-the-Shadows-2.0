using UnityEngine;
using System.Collections;
using Scurge.Player;
using Scurge.Enemy;

namespace Scurge.Util {
	public class DestroyOnHit : MonoBehaviour {

		public float TimeAfterDeath;
		public bool HasExplosion = false;
		public bool Hurt = true;
		public ParticleSystem particle;
		public AudioSource hit;
		public EnemyStats EnemyStats;

		public int MinDamage;
		public int MaxDamage;

		private bool HasHit = false;

		void Start() {
			particle.Stop();
		}

		void OnCollisionEnter(Collision collision) {
			if(collision.gameObject.tag == "Enemy") {
				if(HasExplosion && particle != null) {
					particle.Play();
					if(Hurt) {
						if(collision.gameObject.GetComponent<EnemyStats>() != null) {
							EnemyStats = collision.gameObject.GetComponent<EnemyStats>();
						}
						//For some reason, when hit enemy with projectile, it returns null. Seems to not be searching in children...
						EnemyStats = collision.gameObject.transform.Find("Health").GetComponent<EnemyStats>();
						EnemyStats.DealDamage(Random.Range(MinDamage,MaxDamage), false);
						EnemyStats = null;
					}
					Destroy(gameObject, 0.5f);
				}
				else {
					if(Hurt) {
						collision.gameObject.transform.parent.GetComponentInChildren<EnemyStats>().DealDamage(Random.Range(MinDamage,MaxDamage), false);
					}
					Destroy(gameObject);
				}
			}
			else {
				if(HasExplosion && particle != null) {
					particle.Play();
					Destroy(gameObject, 0.5f);
				}
				else {
					Destroy(gameObject, TimeAfterDeath);
				}
			}
			if(!HasHit) {
				hit.Play();
				HasHit = true;
			}
		}
	}
}