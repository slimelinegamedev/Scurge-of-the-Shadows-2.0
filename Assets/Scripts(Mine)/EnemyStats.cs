﻿using UnityEngine;
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

namespace Scurge.Enemy {
	public class EnemyStats : MonoBehaviour {

		public HealthVariables HealthVariables;
		public Objects Objects;
		public Stats Stats;
		public Music Music;
		public Animator Anim;
		public float DeathTimer = 0.0f;
		public float AttackLength = 3;
		public float MinAttackWait = 3;
		public float MaxAttackWait = 5;
		public int CurrentEnemyHealth;
		public int MaxEnemyHealth;
		public int MinDamage;
		public int MaxDamage;
		public AudioSource Hurt;
		public AudioSource Dead;
		public ParticleSystem HurtParticle;
		public ParticleSystem DeathParticle;
		public Light light;
		public GameObject self;
		public int GoldAmount;
		public List<GameObject> Drops;
		public List<GameObject> RareDrops;
		public bool SummonMinions = false;
		public bool CanSummonMinions;
		public List<GameObject> Minions;
		public GameObject SpawnEmitter;
		public int MinMinionSpawnInterval;
		public int MaxMinionSpawnInterval;
		public bool AttackEnemys = false;
		public bool InRange = false;
		public bool CanHit = false;
		public bool CanAttack = false;

		void Start() {
			HurtParticle.Stop();
			DeathParticle.Stop();
			StartCoroutine(CanHurt());
		}

		void Update() {
			if(CanAttack && InRange) {
				Stats.Hurt(Random.Range(MinDamage, MaxDamage));
				CanAttack = false;
			}
			if(self.gameObject.GetComponent<Renderer>().isVisible) {
				StartCoroutine(SpawnMinions());
			}
			else if(!self.gameObject.GetComponent<Renderer>().isVisible) {
				StopCoroutine(SpawnMinions());
			}
		}

		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				InRange = true;
			}
		}
		void OnTriggerExit(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				InRange = false;
			}
		}
		public void DealDamage(int damageAmount, bool RangeEffect = false) {
			if(CanHit && RangeEffect) {
				if(CurrentEnemyHealth > 0) {
					CurrentEnemyHealth -= damageAmount;
					Hurt.Play();
					StartCoroutine(ParticleBlast(HurtParticle, 0.2f));
					StartCoroutine(HurtFlash());
				}
				else if(CurrentEnemyHealth <= 0) {
					Die(Dead, DeathTimer, DeathParticle);
				}
			}
			if(!RangeEffect) {
				if(CurrentEnemyHealth >= 1) {
					CurrentEnemyHealth -= damageAmount;
					Hurt.Play();
					StartCoroutine(ParticleBlast(HurtParticle, 0.2f));
					StartCoroutine(HurtFlash());
				}
				else if(CurrentEnemyHealth < 1) {
					Die(Dead, 0.5f, DeathParticle);
				}
			}
		}
		public void Die(AudioSource sound, float timer, ParticleSystem particle) {

			foreach(GameObject curDrop in Drops) {
				for(int DropGold = 0; DropGold < GoldAmount; DropGold++) {
					Instantiate(curDrop, transform.position, Quaternion.identity);
				}
			}
			foreach(GameObject curRareDrop in RareDrops) {
				int drop = Random.Range(0, 50);
				if(drop > 45) {
					Instantiate(curRareDrop, transform.position, Quaternion.identity);
				}
			}

			sound.Play();
			particle.Play();

//			transform.parent.gameObject.AddComponent<Rigidbody>().AddForce(Objects.Player.transform.forward);
			Destroy(self, timer);
		}
		IEnumerator ParticleBlast(ParticleSystem particles, float WaitTime) {
			print("Starting Party");
			HurtParticle.Play();
			yield return new WaitForSeconds(WaitTime);
			print("Stopping Party");
			HurtParticle.Stop();
		}
		IEnumerator CanHurt() {
			while(true) {
				CanAttack = false;
				print(gameObject.name + " Cant Hit!");
				yield return new WaitForSeconds(Random.Range(MinAttackWait, MaxAttackWait));
				CanAttack = true;
				Anim.SetTrigger("Attack");
				yield return new WaitForSeconds(AttackLength);
				Anim.SetTrigger("Attack");
				print(gameObject.name + " Can Hit!");
				yield return new WaitForSeconds(Random.Range(MinAttackWait, MaxAttackWait));
			}
		}
		IEnumerator HurtFlash() {
			light.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			light.gameObject.SetActive(false);
		}
		IEnumerator SpawnMinions() {
			while(true) {
				print("Trying To Spawn!");
				if(CanSummonMinions) {
					Vector3 lastSpawnPos = transform.position + new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3));
					var lastMinion = (GameObject)Instantiate(Minions[Random.Range(0, Minions.Count)], lastSpawnPos, transform.rotation);
					lastMinion.SetActive(true);
					var lastParticle = (GameObject)Instantiate(SpawnEmitter, lastSpawnPos, Quaternion.identity);
					lastParticle.SetActive(true);
				}
				yield return new WaitForSeconds(Random.Range(MinMinionSpawnInterval, MaxMinionSpawnInterval));
			}
		}
	}
}