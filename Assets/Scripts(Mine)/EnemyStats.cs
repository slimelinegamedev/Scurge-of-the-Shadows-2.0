using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;

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
					Die(Dead, 0.0f, DeathParticle);
				}
			}
		}
		public void Die(AudioSource sound, float timer, ParticleSystem particle) {
			sound.Play();
			particle.Play();
			
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
	}
}