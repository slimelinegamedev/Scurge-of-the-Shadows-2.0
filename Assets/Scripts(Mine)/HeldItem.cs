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
		public Item item;
		public AudioSource Sound;
		public Objects Objects;
		public bool DestroyOnUse = false;
		public bool LimitedUses = false;
		public int MaxUses = 0;
		public int Uses = 0;
		public AudioSource DestroySound;
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
			//Appends forever. Append only when it changes, and delete old value
			if(LimitedUses) {
				Inventory.ItemDescription[(int)item] = Inventory.ItemDescription[(int)item] + "\n\n" + MaxUses + "/" + Uses;
			}
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
					if(LimitedUses) {
						if(Uses > 0) {
							Uses -= 1;
						}
						else if(Uses <= 0) {
							if(DestroySound != null) {
								DestroySound.Play();
							}
							Inventory.Delete(0, InventoryBar.Equipped);
						}
					}
					if(DestroyOnUse) {
						if(DestroySound != null) {
							DestroySound.Play();
						}
						Inventory.Delete(0, InventoryBar.Equipped);
					}
					EnemyStats = null;
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
					if(LimitedUses) {
						if(Uses > 0) {
							Uses -= 1;
						}
						else if(Uses <= 0) {
							if(DestroySound != null) {
								DestroySound.Play();
							}
							Inventory.Delete(0, InventoryBar.Equipped);
						}
					}
					if(DestroyOnUse) {
						if(DestroySound != null) {
							DestroySound.Play();
						}
						Inventory.Delete(0, InventoryBar.Equipped);
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