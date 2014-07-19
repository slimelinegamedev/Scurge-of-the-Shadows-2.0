﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Scoreboard;
using Scurge.Environment;

namespace Scurge.Player {
	public class Stats : MonoBehaviour {

		public Objects Objects;
		public Disable Disable;
		public Inventory Inventory;
		public Dungeon Dungeon;
		public Highscore Highscore;

		public int Health;
		public int Gold;
		public int Defense;
		public int Attack;
		public int Magic;
		public int Experience;
		
		public int Level;		

		public int ExperienceLevel;
		public int MaxHealth;

		public AudioSource LevelUpSound;
		public AudioSource HurtSound;
		public bool Dead = false;
		public int Countdown = 10;
		public GUISkin Skin;

		void Start() {
			Highscore = GameObject.Find("Highscore").GetComponent<Highscore>();
		}

		void Update() {
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
		}

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

		public void GiveExperience(int amount) {
			if(Experience < ExperienceLevel) {
				Experience += amount;
			}
			else if(Experience >= ExperienceLevel) {
				LevelUp();
			}
		}

		public void AddDefense(int amount) {
			Defense += amount;
		}

		public void AddMagic(int amount) {
			Magic += amount;
		}

		public void LevelUp() {
			Experience = 0;
			ExperienceLevel += 50;
			LevelUpSound.Play();
		}

		public void Die() {
			Dead = true;
			Disable.DisableObj(true);
			Highscore.add("Player1", Dungeon.Floor, Gold, "SOMETHING");
			Objects.Player.transform.position = new Vector3(-1000000, -1000000, -1000000);
		}

		void OnGUI() {
			GUI.skin = Skin;
			GUI.depth = 2;
			if(!Inventory.InventoryOpen) {
				GUI.Label(new Rect(10, 690, 100, 25), Health + " / " + MaxHealth, "Health");
			}
			if(Dead) {
				GUI.Box(new Rect(0, 0, 1280, 720), "", "DeathBox");
				GUI.Label(new Rect(0, 50, 1280, 720), "Continue?", "DeathText");
				GUI.Label(new Rect(0, 300, 1280, 720), Countdown.ToString(), "DeathText");
				if(GUI.Button(new Rect(435, 595, 500, 75), "Play Again", "DeathTextNoCenter")) {
					Application.LoadLevel(1);
				}
			}
		}

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
	}
}