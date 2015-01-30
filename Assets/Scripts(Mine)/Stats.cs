using UnityEngine;
using UnityEngine.UI;
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

namespace Scurge.Player {
	public class Stats : MonoBehaviour {

		#region Vars
		public Objects Objects;
		public Disable Disable;
		public Inventory Inventory;
		public Dungeon Dungeon;
		public Highscore Highscore;
		public int Health;
		public int Gold;
		public int Defense;
		public int Attack;
		public int Mana = 100;
		public int Experience;
		public int Level;
		public int ExperienceLevel;
		public int MaxHealth;
		public int MaxMana = 100;
		public float ManaRegenWait = 5;
		public int ManaRegenAmount = 10;
		public AudioSource LevelUpSound;
		public AudioSource HurtSound;
		public bool Dead = false;
		public int Countdown = 10;
		public GUISkin Skin;

		public Slider HealthSlider;
		public Slider ManaSlider;
		public Text goldText;

		public Canvas deathCanvas;
		public Text deathCountDownText;

		public bool CanUseSpell(int ManaCost) {
			if(Mana  - ManaCost >= 0) {
				return true;
			}
			else {
				return false;
			}
		}
		#endregion

		#region Unity Methods

		void Awake() {
			StartRegen();
		}

		void Start() {
			Highscore = GameObject.Find("Highscore").GetComponent<Highscore>();
		}

		void Update() {
			if(Mana <= 0) {
				Mana = 0;
			}
			if(Mana > MaxMana) {
				Mana = MaxMana;
			}
			if(Health <= 0) {
				Die();
			}
			if(Health > MaxHealth) {
				Health = MaxHealth;
			}
			if(Dead) {
				bool HasDied = false;
				Screen.showCursor = true;
				Screen.lockCursor = false;
				if(!HasDied) {
					StartCoroutine(CountdownTimer());
					HasDied = true;
				}
				if(Countdown <= 0) {
					print("Why didnt you continue?");
					Application.LoadLevel(0);
				}
			}

			HealthSlider.value = Health;
			HealthSlider.maxValue = MaxHealth;

			ManaSlider.value = Mana;
			ManaSlider.maxValue = MaxMana;

			goldText.text = Gold.ToString();

			if(Dead && !deathCanvas.enabled) {
				deathCanvas.enabled = true;
			}
			deathCountDownText.text = Countdown.ToString();
		}
		#endregion

		#region Upgrading
		public void UpgradeManaRegenWait(float amount) {
			ManaRegenWait = amount;
		}
		public void AddDefense(int amount) {
			Defense += amount;
		}
		public void LevelUp() {
			ExpUpgrade(0, ExperienceLevel+50);
			ManaUpgrade(0, MaxMana+25);
			LevelUpSound.Play();
		}
		public void ManaUpgrade(int ManaSet, int MaxManaSet) {
			Mana = ManaSet;
			MaxMana = MaxManaSet;
		}
		public void ExpUpgrade(int ExperienceSet, int ExperienceLevelSet) {
			Experience = ExperienceSet;
			ExperienceLevel = ExperienceLevelSet;
		}
		#endregion

		#region Giving/Restoring
		public void RegenMana(int amount) {
			Mana = Mana + amount;
		}
		public void GiveGold(int amount) {
			Gold += amount;
		}
		public void GiveExperience(int amount) {
			if(Experience < ExperienceLevel) {
				Experience += amount;
			}
			else if(Experience >= ExperienceLevel) {
				LevelUp();
			}
		}
		public void RestoreMana(int amount) {
			if(amount < MaxMana) {
				Mana += amount;
			}
			if(Mana > MaxMana) {
				Mana = MaxMana;
			}
		}
		public void UseMana(int amount) {
			if(Mana > 0) {
				Mana -= amount;
			}
			else if(Mana <= 0) {
				Debug.LogError("Not Enough Mana");
			}
		}
		public void Heal(int amount) {
			if(Health < MaxHealth) {
				Health += amount;
			}
			else if(Health > MaxHealth) {
				Debug.LogError("Trying To Restore Too Much Health");
			}
		}
		#endregion

		#region Dying/Hurting
		public void Hurt(int damage) {
			print("Ouch!");
			if(Health > 0) {
				Health -= damage - Defense / 3;
				HurtSound.Play();
			}
			else if(Health <= 0) {
				print("Dead");
				Die();
			}
		}

		public void Die() {
			Dead = true;
			Disable.DisableObj(true, false);
			Objects.UIOptions.SetActive(false);
			Objects.UIControls.SetActive(false);
			Objects.UIInventory.SetActive(false);
			Objects.CalibrationWindow.SetActive(false);
			Objects.PauseObject.SetActive(false);
			Objects.PauseMenu.SetActive(false);
			Objects.HUD.SetActive(false);
			Highscore.add("Player1", Dungeon.Floor, Gold, "SOMETHING");
			Objects.Player.transform.position = new Vector3(-1000000, -1000000, -1000000);
		}
		#endregion
		public void RestartLevel() {
			Application.LoadLevel(1);
		}
		#region Script Space Takers
		IEnumerator CountdownTimer() {
			if(true) {
				yield return new WaitForSeconds(1);
				Countdown = 9;
				yield return new WaitForSeconds(1);
				Countdown = 8;
				yield return new WaitForSeconds(1);
				Countdown = 7;
				yield return new WaitForSeconds(1);
				Countdown = 6;
				yield return new WaitForSeconds(1);
				Countdown = 5;
				yield return new WaitForSeconds(1);
				Countdown = 4;
				yield return new WaitForSeconds(1);
				Countdown = 3;
				yield return new WaitForSeconds(1);
				Countdown = 2;
				yield return new WaitForSeconds(1);
				Countdown = 1;
				yield return new WaitForSeconds(1);
				Countdown = 0;
			}
		}

		public void StartRegen() {
			InvokeRepeating("ManaRegen", 2, ManaRegenWait);
		}
		
		void ManaRegen() {
			print("Regening Mana Part 1!");
			RegenMana(ManaRegenAmount);
			print("Regening Mana Wait!");
		}

		#endregion
	}
}