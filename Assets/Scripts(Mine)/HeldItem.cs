using UnityEngine;
using System.Collections;
using Scurge.Util;
using Scurge.Player;
using Scurge.Enemy;
using TeamUtility.IO;

namespace Scurge.Player {

	public enum Type {
		Melee = 0,
		SpellOrThrow = 2,
		Nothing = 3
	}

	public enum Side {
		Left = 0,
		Right = 1
	}

	public enum Command {
		Spell = 0,
		Melee = 1,
		Throw = 3,
		Nothing = 4
	}

	public class HeldItem : MonoBehaviour {
		public Inventory Inventory;
		public Stats Stats;
		public EnemyStats EnemyStats;
		public Type ItemType;
		public Spell spell;
		public Side ItemSide;
		public Command ItemCommand;
		public Item item;
		public GUISkin Skin;
		public AudioSource Sound;
		public Objects Objects;
		public bool DestroyOnUse = false;
		public bool LimitedUses = false;
		public int MaxUses = 0;
		public int Uses = 0;
		public bool ShowUses = false;
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
		public Pause Pause;

		//TODO: Make cooldown work when not hitting the enemy

		public void ChangeSpell(Spell spellToChangeTo) {
			spell = spellToChangeTo;
		}

		void Update() {
			if(LimitedUses) {
				if(Inventory.HasEquipped(item)) {
					ShowUses = true;
				}
			}
			if(!Pause.Open) {
				if(ItemSide == Side.Right) {
					if(cInput.GetKeyDown("Attack/Fire 2") && !Inventory.InventoryOpen) {

						if(ItemCommand != Command.Spell) {
							Sound.Play();
						}

						if(!Cooldown) {
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
							else if(ItemCommand == Command.Throw) {
								ThrowProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.7f, 0), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
							}
							else if(ItemCommand == Command.Spell) {
								UseSpell(spell);
							}
						}
						else if(Cooldown) {
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
							else if(ItemCommand == Command.Throw) {
								ThrowProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.7f, 0), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
							}
							else if(ItemCommand == Command.Spell) {
								UseSpell(spell);
							}
							StartCoroutine(SwingCool(CooldownTime));
						}
						if(HasAnimation) {
							if(CanSwing) {
								SwingAnimation.Play();
							}
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
					if(cInput.GetKeyDown("Attack/Fire 1") && !Inventory.InventoryOpen) {

						if(ItemCommand != Command.Spell) {
							Sound.Play();
						}

						if(!Cooldown) {
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
							else if(ItemCommand == Command.Throw) {
								ThrowProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.7f, 0), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
							}
							else if(ItemCommand == Command.Spell) {
								UseSpell(spell);
							}
						}
						else if(Cooldown) {
							if(ItemType == Type.Melee) {
								EnemyStats.DealDamage(Random.Range(DamageMin, DamageMax));
							}
							else if(ItemCommand == Command.Throw) {
								ThrowProjectile(Projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.7f, 0), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
							}
							else if(ItemCommand == Command.Spell) {
								UseSpell(spell);
							}
							StartCoroutine(SwingCool(CooldownTime));
						}
						if(HasAnimation) {
							if(CanSwing) {
								SwingAnimation.Play();
							}
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
		}

		void OnGUI() {
			GUI.skin = Skin;
			if(ShowUses) {
				GUI.Label(new Rect(1200, 690, 100, 25), Uses + " / " + MaxUses);
			}
		}

		void ThrowProjectile(GameObject Projectile, Vector3 Position, Vector3 Direction, int Power) {
			var NewProjectile = (GameObject)Instantiate(Projectile, Position, transform.rotation);
			NewProjectile.transform.rigidbody.AddForce(Direction * Power);
		}

		public GameObject lastSpawnedParticle;

		public void UseSpell(Spell spellToUse) {
			if(Stats.CanUseSpell(spellToUse.ManaCost)) {
				spellToUse.sound.Play();
				Stats.UseMana(spell.ManaCost);

				if(spellToUse.hasParticle) {
					lastSpawnedParticle = (GameObject)Instantiate(spellToUse.particle, transform.position + spellToUse.positionAdder, Quaternion.identity);
					lastSpawnedParticle.SetActive(true);
					if(spellToUse.attachParticleToPlayer) {
						lastSpawnedParticle.transform.parent = Objects.Player.transform;
					}
				}

				#region Spell Type Dependent
				if(spellToUse.spellType == SpellType.Heal) {
					Stats.Heal(spell.healAmount);
				}
				if(spellToUse.spellType == SpellType.Projectile) {
					ThrowProjectile(spell.projectile, Objects.Player.transform.position + Objects.Camera.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.7f, 0), Objects.Camera.transform.TransformDirection(Vector3.forward), 2000);
				}
				if(spellToUse.spellType == SpellType.ExpandingSphere) {
					var lastSphere = (GameObject)Instantiate(spellToUse.particle, Objects.Player.transform.position, Quaternion.identity);
					spellToUse.sphereToExpand = lastSpawnedParticle.GetComponent<SphereCollider>();
					while(spellToUse.sphereToExpand.radius < spellToUse.radiusToExpand) {
						spellToUse.sphereToExpand.radius++;
					}
					if(spellToUse.attachParticleToPlayer) {
						lastSphere.transform.parent = Objects.Player.transform;
					}
					lastSphere.SetActive(true);
					spellToUse.particle.GetComponent<ParticleSystem>().startLifetime = spellToUse.deflectionDuration - 2;
					lastSphere.GetComponent<DestroyAfterWait>().wait = spellToUse.deflectionDuration;
				}
				if(spellToUse.spellType == SpellType.Nothing) {
					Debug.Log("Spell " + spell.name + " Has No Type");
				}
				#endregion
				print("Using Spell!");
			}
		}

		IEnumerator Wait(float time) {
			yield return new WaitForSeconds(time);
		}

		IEnumerator SwingCool(float Wait) {
			print("Cooling Down");
			CanSwing = false;
			yield return new WaitForSeconds(Wait);
			CanSwing = true;
		}
	}
}