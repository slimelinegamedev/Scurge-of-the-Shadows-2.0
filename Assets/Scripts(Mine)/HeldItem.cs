using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	public enum SpellShape {
		AreaOfEffect,
		Beam,
		Binding,
		Channel,
		Projectile,
		Self,
		Touch,
		Wall
	}

	public enum SpellAttackType {
		Earth,
		Fire,
		Water,
		Air,
		Time,
		Electricity,
		Poison,
		Heal
	}

	[System.Serializable]
	public class CustomSpell {
		public string EditorLabel = "Label";

		public SpellShape shape;
		public SpellAttackType attackType;
		public AudioSource sound;

		public bool SHOWING_IN_EDITOR = false;
	}

	public class HeldItem : MonoBehaviour {

		public Inventory Inventory;
		public Stats Stats;
		public EnemyStats EnemyStats;
		public Type ItemType;
		public bool IsStaff = false;
		public Spell spell;

		public bool UsingCustomSpells = false;
		public GameObject StaffParticle;
		public List<CustomSpell> CustomSpells = new List<CustomSpell>() {
			new CustomSpell()
		};
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

		public bool usingMasterSound = false;
		public AudioSource masterSound;
		public GameObject currentStaffParticle;
		public bool HasPutStaffParticle = false;

		//TODO: Make cooldown work when not hitting the enemy
		//TODO: Make the new spells actually work now

		public void ChangeSpell(Spell spellToChangeTo) {
			Inventory.curStaff.UsingCustomSpells = false;
			Inventory.curStaff.CustomSpells = new List<CustomSpell>() {
				new CustomSpell()
			};
			spell = spellToChangeTo;
		}

		public void ChangeCustomSpells(HeldItem heldItem) {
			Inventory.curStaff.UsingCustomSpells = true;
			Inventory.curStaff.spell = null;
			Inventory.curStaff.CustomSpells = heldItem.CustomSpells;

			GameObject newStaffParticle = (GameObject)Instantiate(heldItem.StaffParticle, Vector3.zero, heldItem.StaffParticle.transform.rotation);
			newStaffParticle.transform.parent = Inventory.curStaff.transform;
			newStaffParticle.transform.localPosition = new Vector3(0, 0.00158f, 0);

			Inventory.curStaff.currentStaffParticle = newStaffParticle;
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
							if(!UsingCustomSpells) {
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
							else {
								UseCustomSpell(this);
							}
						}
						else if(Cooldown) {
							if(!UsingCustomSpells) {
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
							else {
								UseCustomSpell(this);
							}
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
							if(!UsingCustomSpells) {
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
							else {
								UseCustomSpell(this);
							}
						}
						else if(Cooldown) {
							if(!UsingCustomSpells) {
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
							else {
								UseCustomSpell(this);
							}
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

		//TODO: Make the uses show as 4.6 UI
		void OnGUI() {
			GUI.skin = Skin;
			if(ShowUses) {
				GUI.Label(new Rect(1200, 690, 100, 25), Uses + " / " + MaxUses);
			}
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

		public void UseCustomSpell(HeldItem heldItem) {
			if(heldItem.usingMasterSound) {
				heldItem.masterSound.Play();
			}
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.shape) {
					case SpellShape.AreaOfEffect:
						//AoE
						AreaOfEffectSpell(heldItem);
						break;
					
					case SpellShape.Beam:
						//Beam
						BeamSpell(heldItem);
						break;

					case SpellShape.Binding:
						//Binding
						BindingSpell(heldItem);
						break;

					case SpellShape.Channel:
						//Channel
						ChannelSpell(heldItem);
						break;

					case SpellShape.Projectile:
						//Projectile
						ProjectileSpell(heldItem);
						break;

					case SpellShape.Self:
						//Self
						SelfSpell(heldItem);
						break;

					case SpellShape.Touch:
						//Touch
						TouchSpell(heldItem);
						break;

					case SpellShape.Wall:
						//Wall
						WallSpell(heldItem);
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		#region Custom Spell Functions

		public void AreaOfEffectSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void BeamSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void BindingSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void ChannelSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void ProjectileSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void SelfSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void TouchSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		public void WallSpell(HeldItem heldItem) {
			foreach(CustomSpell customSpell in heldItem.CustomSpells) {
				switch(customSpell.attackType) {
					case SpellAttackType.Air:
						//Air
						break;

					case SpellAttackType.Earth:
						//Earth
						break;

					case SpellAttackType.Electricity:
						//Electricity
						break;

					case SpellAttackType.Fire:
						//Fire
						break;

					case SpellAttackType.Water:
						//Water
						break;

					case SpellAttackType.Heal:
						//Heal
						break;

					case SpellAttackType.Poison:
						//Poison
						break;

					case SpellAttackType.Time:
						//Time
						break;

					default:
						Debug.LogError("How Did You Manage This Bug? Tell Me How On GitHub!");
						return;
				}
			}
		}

		#endregion

		void ThrowProjectile(GameObject Projectile, Vector3 Position, Vector3 Direction, int Power) {
			var NewProjectile = (GameObject)Instantiate(Projectile, Position, transform.rotation);
			NewProjectile.transform.GetComponent<Rigidbody>().AddForce(Direction * Power);
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