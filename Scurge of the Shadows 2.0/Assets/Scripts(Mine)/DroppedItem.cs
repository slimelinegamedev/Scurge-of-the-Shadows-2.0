using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;

namespace Scurge.Player {
	public class DroppedItem : MonoBehaviour {

		public Inventory Inventory;

		public Item type;
		public TextMesh text;
		public AudioSource pickup;
		public GameObject parent;

		private bool hoveringOver = false;

		void Start() {

		}

		void Update() {
			if(hoveringOver) {
				text.gameObject.SetActive(true);
				if(Input.GetKeyDown(KeyCode.Q) && !Inventory.Full) {
					Inventory.Give(type, Inventory.FirstOpenSlot, InventoryBar.Inventory);
					pickup.Play();
					Destroy(parent);
				}
			}
			else {
				text.gameObject.SetActive(false);
			}
		}

		void OnMouseOver() {
			hoveringOver = true;
		}

		void OnMouseExit() {
			hoveringOver = false;
		}
	}
}