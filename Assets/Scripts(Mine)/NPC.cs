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
using TeamUtility.IO;

namespace Scurge.Environment {

	[System.Serializable]
	public class Trade {
		[Tooltip("The name to show for the trade")]
		public string name;
		[Tooltip("The amount of gold it costs to buy this item")]
		public int cost;
		[Tooltip("The item they are selling")]
		public Item item;
		[Tooltip("The amount of the item they have")]
		public int amount;
		[Tooltip("The rectangle for the button")]
		public Rect rectangle;
	}

	public class NPC : MonoBehaviour {
		[Tooltip("GUI Skin")]
		public GUISkin Skin;
		[Tooltip("The Stats Script")]
		public Stats Stats;
		[Tooltip("The Inventory Script")]
		public Inventory Inventory;
		[Tooltip("The Disable Script")]
		public Disable Disable;
		[Tooltip("All there trades")]
		public List<Trade> trades;
		[Tooltip("Is the player close enough to be trading with them?")]
		public bool CloseEnough = false;
		[Tooltip("Is the player trading with them?")]
		public bool Interacting = false;

		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				CloseEnough = true;
			}
		}
		void OnTriggerExit(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				CloseEnough = false;
			}
		}
		void Update() {
			if(CloseEnough) {
				if(Input.GetKeyDown(KeyCode.Q)) {
					Interacting = true;
					Disable.DisableObj(true, false);
				}
			}
			if(Interacting) {
				Screen.showCursor = true;
				Screen.lockCursor = false;
			}
		}
		void OnGUI() {
			GUI.skin = Skin;
			if(CloseEnough && !Interacting) {
				GUI.Label(new Rect(10, 10, 500, 500), "<size=20>Press " + InputManager.GetAxisConfiguration("Default", "Interact").positive.ToString() + " To Trade!</size>");
			}
			if(Interacting) {
				GUI.Box(new Rect(415, 110, 450, 500), "Trades");
				if(GUI.Button(new Rect(425, 140, 430, 50), "Exit")) {
					Disable.EnableObj(true, false);
					Screen.showCursor = false;
					Screen.lockCursor = true;
					Interacting = false;
				}
				foreach(Trade curTrade in trades) {
					if(curTrade.amount > 0) {
						if(GUI.Button(curTrade.rectangle, curTrade.name + " For " + curTrade.cost.ToString() + " Gold, " + curTrade.amount.ToString() + " Left")) {
							if(Stats.Gold >= curTrade.cost) {
								Inventory.Give(curTrade.item, Inventory.FirstOpenSlot, InventoryBar.Inventory);
								Stats.Gold -= curTrade.cost;
								curTrade.amount -= 1;
							}
						}
					}
				}
			}
		}
	}
}