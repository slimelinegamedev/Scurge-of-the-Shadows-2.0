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
using Scurge.Scoreboard;

namespace Scurge.Environment {
	public class Chest : MonoBehaviour {

		public Inventory Inventory;
		public AudioSource LootSound;

		public GUISkin Skin;
		public bool LookingIn = false;
		public bool CanHave = true;

		public Item item;

		void Start() {
			item = (Item)Random.Range(1, Inventory.ItemDescription.Count);
		}
		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				LookingIn = true;
			}
		}
		void OnTriggerExit(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				LookingIn = false;
			}
		}
		void OnGUI() {
			GUI.skin = Skin;
			if(LookingIn && CanHave) {
				Inventory.Give(item, Inventory.FirstOpenSlot, InventoryBar.Inventory);
				item = Item.None;
				LootSound.Play();
				CanHave = false;
			}
		}
	}
}