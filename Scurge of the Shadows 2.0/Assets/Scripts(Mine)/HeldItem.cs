using UnityEngine;
using System.Collections;
using Scurge.Util;
using Scurge.Player;
using Scurge.Enemy;

namespace Scurge.Player {

	public enum Type {
		Melee = 0,
		Staff = 1,
		Nothing = 2
	}

	public enum Side {
		Left = 0,
		Right = 1
	}

	public enum Command {
		Projectile = 0,
		Melee = 1,
		Summon = 2,
		Nothing = 3
	}

	public class HeldItem : MonoBehaviour {
		public Inventory Inventory;
		public EnemyStats EnemyStats;
		public Type ItemType;
		public Side ItemSide;
		public Command ItemCommand;
		public AudioSource Sound;
		public Objects Objects;
		public GameObject Projectile;
		public GameObject EnemyHit;
		public int DamageMin = 1;
		public int DamageMax = 3;
		public bool CanSwing ;
		public bool Cooldown;
		public float CooldownTime = 0.5f;
		public bool HasAnimation;
		public Animation SwingAnimation;


		void Update() {
			if(ItemSide == Side.Right) {
				if(Input.GetMouseButtonDown(1) && !Inventory.InventoryOpen) {
					if(!Cooldown) {
						Sound.Play();
						if(ItemType == Type.Melee) {
							EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
						}
					}
					if(HasAnimation && CanSwing) {
						SwingAnimation.Play();
						if(Cooldown) {
							Sound.Play();
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
							StartCoroutine(SwingCool(CooldownTime));
						}
					}
					if(ItemCommand == Command.Projectile) {
						ShootProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
					}
				}
			}
			if(ItemSide == Side.Left) {
				if(Input.GetMouseButtonDown(0) && !Inventory.InventoryOpen) {
					if(!Cooldown) {
						Sound.Play();
						if(ItemType == Type.Melee) {
							EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
						}
					}
					if(HasAnimation && CanSwing) {
						SwingAnimation.Play();
						if(Cooldown) {
							Sound.Play();
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
						}
						StartCoroutine(SwingCool(CooldownTime));
					}
					if(ItemCommand == Command.Projectile) {
						ShootProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
					}
					EnemyStats = null;
				}
			}
		}

		void ShootProjectile(GameObject Projectile, Vector3 Position, Vector3 Direction, int Power) {
			var NewProjectile = (GameObject)Instantiate(Projectile, Position, transform.rotation);
			NewProjectile.transform.rigidbody.AddForce(Direction * Power);
		}

		IEnumerator SwingCool(float Wait) {
			print("Cooling Down");
			CanSwing = false;
			yield return new WaitForSeconds(Wait);
			CanSwing = true;
		}
	}
}