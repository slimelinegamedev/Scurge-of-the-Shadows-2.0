using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

namespace Scurge.Player {

	public enum Potion {
		None = 0,
		Health = 1,
		Damage = 2
	}
	public enum ItemType {
		None = 0,
		Blade = 1,
		Staff = 2,
		Ring = 3,
		Spell = 4,
		Helmet = 5,
		Chestplate = 6,
		Potion = 7
	}

	public enum InventoryBar {
		Inventory = 0,
		Equipped = 1
	}
	
	public enum Item {
		None = 0,
		DaggerBasic  = 1,
		StaffBasic = 2,
		SwordTough = 3,
		AxeBattle = 4,
		RingRed = 5
	}

	public enum Ring {
		None = 0,
		Attack = 1,
		Defense = 2,
		Magic = 3,
		Health = 4
	}

	public class Inventory : MonoBehaviour {
		public Stats Stats;
		public Objects Objects;
		public bool Visible;
		public bool InventoryOpen;
		public Texture2D Crosshair;
		public float SubtractionX;
		public float SubtractionY;
		public Rect BackgroundRect;
		public Disable Disable;
		public bool Moving;
		public Texture2D movingTex;
		public Item movingItem;
		public Item lastMovedItem;
		public ItemType curItemType;
		public int FirstOpenSlot;
		public bool Full = false;
		public int ActiveItem;
		public GUISkin Skin;

		public List<Item> Items;
		public List<Item> EquippedItems;
		public List<ItemType> Types;
		public List<string> ItemDescription;
		public List<GameObject> ItemObjects;
		public List<GameObject> ThrownObjects;
		public List<Texture2D> InventoryTextures;
		public List<Texture2D> EquippingEmptyTextures;
		//Variables for other item types
		public List<Ring> RingTypes;
		public List<int> DefenseAmounts;
		public List<int> StatsAmounts;
		public List<Potion> PotionTypes;
		//End
		public List<string> TooltipText;

		private bool HasFound = false;

		public void Give(Item item, int slot, InventoryBar bar) {
			if(!Full) {
				if(bar == InventoryBar.Inventory) {
					Items[slot] = item;
				}
				else if(bar == InventoryBar.Equipped) {
					EquippedItems[slot] = item;
				}
			}
		}

		public void Move(Item curItem, Texture2D curItemTex,  int slot, InventoryBar bar, ItemType type) {
			print("Moving");
			if(bar == InventoryBar.Inventory && Moving == false) {
				print("Moving : Made It Past Checkup");
				lastMovedItem = curItem;
				Items[slot] = Item.None;
				Moving = true;
				movingTex = curItemTex;
				movingItem = curItem;
				curItemType = type;
			}
			else if(bar == InventoryBar.Equipped && !Moving){
				print("InventoryBar.Equipped");
				print("Moving : Made It Past Checkup");
				lastMovedItem = curItem;
				EquippedItems[slot] = Item.None;
				Moving = true;
				movingTex = curItemTex;
				movingItem = curItem;
				curItemType = type;
			}
		}

		public void EquipItems() {

			ItemObjects[(int)EquippedItems[0]].SetActive(true);

			ActiveItem = (int)EquippedItems[0];

			for(int allItemsCycle = 0; allItemsCycle < ItemObjects.Count; allItemsCycle++) {
				if(allItemsCycle != ActiveItem) {
					ItemObjects[allItemsCycle].SetActive(false);
				}
			}
		}

		public void ThrowItem(Item thrower) {
			var ThrownItem = (GameObject)Instantiate(ThrownObjects[(int)thrower], transform.position, transform.rotation);
			ThrownItem.transform.rigidbody.AddForce(Objects.Camera.transform.TransformDirection(Vector3.forward) * 500);
		}

		public void FindSlot() {
			FirstOpenSlot = Items.IndexOf(Item.None);
			if(FirstOpenSlot == -1) {
				Full = true;
			}
			else {
				Full = false;
			}
		}

		public void Place(int slot, InventoryBar bar) {
			if(bar == InventoryBar.Inventory) {
				print("Placing InventoryBar.Inventory[" + slot.ToString() + "]");
				lastMovedItem = Item.None;
				Items[slot] = movingItem;
				Moving = false;
				movingItem = Item.None;
				movingTex = null;
			}
			else if(bar == InventoryBar.Equipped) {
				print("Placing InventoryBar.Equipped[" + slot.ToString() + "]");
				lastMovedItem = Item.None;
				EquippedItems[slot] = movingItem;
				Moving = false;
				movingItem = Item.None;
				movingTex = null;
			}
		}

		public void Swap(int slot, InventoryBar bar) {
			if(bar == InventoryBar.Inventory) {
				movingItem = Items[slot];
				movingTex = InventoryTextures[(int)Items[slot]];
				Items[slot] = lastMovedItem;
			}
			else if(bar == InventoryBar.Equipped) {
				print("InventoryBar.Equipped");
				movingItem = EquippedItems[slot];
				movingTex = InventoryTextures[(int)EquippedItems[slot]];
				EquippedItems[slot] = lastMovedItem;
			}
		}

		public void SlotHandle(int slot, InventoryBar barbar) {
			if(barbar == InventoryBar.Inventory) {
				print("Picked up on InventoryBar.Inventory");
				if(Moving == false && Items[slot] != Item.None) {
					Move(Items[slot], InventoryTextures[(int)Items[slot]], slot, InventoryBar.Inventory, Types[(int)Items[slot]]);
				}
				else if(Moving && Items[slot] == Item.None) {
					print("Placed Item On InventoryBar.Inventory");
					Place(slot, InventoryBar.Inventory);
				}
				else if(Moving && Items[0] != Item.None) {
					print("Swapped Item For First Time On InventoryBar.Inventory");
					Swap(slot, InventoryBar.Inventory);
				}
			}
			else if(barbar == InventoryBar.Equipped) {
				print("Picked up on InventoryBar.Equipped");
				if(!Moving && EquippedItems[slot] != Item.None) {
					Move(EquippedItems[slot], InventoryTextures[(int)EquippedItems[slot]], slot, InventoryBar.Equipped, Types[(int)EquippedItems[slot]]);
				}
				else if(Moving && EquippedItems[slot] == Item.None) {
					print("Placed Item On InventoryBar.Equipped");
					Place(slot, InventoryBar.Equipped);
				}
				else if(Moving && EquippedItems[slot] != Item.None) {
					print("Swapped Item For First Time On InventoryBar.Equipped");
					Swap(slot, InventoryBar.Equipped);
				}
			}
		}

		public void ApplyStats() {
			//Ring stats application
			//Temporary reset method
			Stats.MaxHealth = 10;
			if(Stats.Health >= Stats.MaxHealth) {
				Stats.Health = Stats.MaxHealth;
			}
			Stats.Magic = 0;
			Stats.Defense = 0;
			Stats.Attack = 0;
			for(int allRings = 0; allRings < RingTypes.Count; allRings++) {
				if(RingTypes[allRings] != Ring.None) {
					if(RingTypes[allRings] == Ring.Attack) {
						if(EquippedItems[2] == (Item)allRings) {
							Stats.Attack = StatsAmounts[allRings];
						}
					}
					if(RingTypes[allRings] == Ring.Defense) {
						if(EquippedItems[2] == (Item)allRings) {
							Stats.Defense = StatsAmounts[allRings];
						}
					}
					if(RingTypes[allRings] == Ring.Magic) {
						if(EquippedItems[2] == (Item)allRings) {
							Stats.Magic = StatsAmounts[allRings];
						}
					}
					if(RingTypes[allRings] == Ring.Health) {
						if(EquippedItems[2] == (Item)allRings) {
							Stats.MaxHealth = StatsAmounts[allRings];
							if(Stats.Health == Stats.MaxHealth) {
								Stats.Health = StatsAmounts[allRings];
							}
						}
					}
				}
			}
		}

		void Update() {
			if(!InventoryOpen) {
				Screen.showCursor = false;
				Screen.lockCursor = true;
			}
			else {
				Screen.showCursor = true;
				Screen.lockCursor = false;
			}
			FindSlot();
			EquipItems();
			ApplyStats();

			if(Input.GetKeyDown(KeyCode.E) && !Moving) {
				InventoryOpen = !InventoryOpen;
				if(InventoryOpen) {
					Screen.showCursor = true;
					Screen.lockCursor = false;
					Disable.DisableObj(false);
				}
				else {	
					Screen.showCursor = false;
					Screen.lockCursor = true;
					Disable.EnableObj();
				}
			}
			//Give item tooltips
			TooltipText[0] = ItemDescription[(int)Items[0]];
			TooltipText[1] = ItemDescription[(int)Items[1]];
			TooltipText[2] = ItemDescription[(int)Items[2]];
			TooltipText[3] = ItemDescription[(int)Items[3]];
			TooltipText[4] = ItemDescription[(int)Items[4]];
			TooltipText[5] = ItemDescription[(int)Items[5]];
			TooltipText[6] = ItemDescription[(int)Items[6]];
			TooltipText[7] = ItemDescription[(int)Items[7]];
			TooltipText[8] = ItemDescription[(int)Items[8]];
			TooltipText[9] = ItemDescription[(int)Items[9]];
			TooltipText[10] = ItemDescription[(int)Items[10]];
			TooltipText[11] = ItemDescription[(int)Items[11]];
			TooltipText[12] = ItemDescription[(int)Items[12]];
			TooltipText[13] = ItemDescription[(int)Items[13]];
			TooltipText[14] = ItemDescription[(int)Items[14]];
			TooltipText[15] = ItemDescription[(int)Items[15]];
			//Equipped
			TooltipText[16] = ItemDescription[(int)EquippedItems[0]];
			TooltipText[17] = ItemDescription[(int)EquippedItems[1]];
			TooltipText[18] = ItemDescription[(int)EquippedItems[2]];
			TooltipText[19] = ItemDescription[(int)EquippedItems[3]];
			TooltipText[20] = ItemDescription[(int)EquippedItems[4]];
		}

		void OnGUI() {
			GUI.skin = Skin;

			if(Visible) {
				if(!InventoryOpen) {
					GUI.DrawTexture(new Rect(Screen.width/2,Screen.height/2 - 50, 20, 20), Crosshair);
				}
				else {
					//Throw Button
					if(Moving) {
						if(GUI.Button(new Rect(0, 0, 440, 720), "", "Label")) {
							ThrowItem(movingItem);
							Moving = false;
						}
						if(GUI.Button(new Rect(840, 0, 440, 720), "", "Label")) {
							ThrowItem(movingItem);
							Moving = false;
						}
						if(GUI.Button(new Rect(440, 585, 400, 135), "", "Label")) {
							ThrowItem(movingItem);
							Moving = false;
						}
						if(GUI.Button(new Rect(440, 0, 400, 135), "", "Label")) {
							ThrowItem(movingItem);
							Moving = false;
						}
					}
					//BackGround Button
					GUI.Box(BackgroundRect, "");
					//Behind Equipped Box
					GUI.Box(new Rect(438 - SubtractionX, 410 - SubtractionY, 340, 84), "");

					//First Row
					if(GUI.Button(new Rect(480 - SubtractionX, 100 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[0]], TooltipText[0]))) {
						SlotHandle(0, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(544 - SubtractionX, 100 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[1]], TooltipText[1]))) {
						SlotHandle(1, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(608 - SubtractionX, 100 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[2]], TooltipText[2]))) {
						SlotHandle(2, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(672 - SubtractionX, 100 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[3]], TooltipText[3]))) {
						SlotHandle(3, InventoryBar.Inventory);
					}
					//Second Row
					if(GUI.Button(new Rect(480 - SubtractionX, 164 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[4]], TooltipText[4]))) {
						SlotHandle(4, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(544 - SubtractionX, 164 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[5]], TooltipText[5]))) {
						SlotHandle(5, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(608 - SubtractionX, 164 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[6]], TooltipText[6]))) {
						SlotHandle(6, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(672 - SubtractionX, 164 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[7]], TooltipText[7]))) {
						SlotHandle(7, InventoryBar.Inventory);
					}
					//Third Row
					if(GUI.Button(new Rect(480 - SubtractionX, 228 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[8]], TooltipText[8]))) {
						SlotHandle(8, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(544 - SubtractionX, 228 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[9]], TooltipText[9]))) {
						SlotHandle(9, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(608 - SubtractionX, 228 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[10]], TooltipText[10]))) {
						SlotHandle(10, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(672 - SubtractionX, 228 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[11]], TooltipText[11]))) {
						SlotHandle(11, InventoryBar.Inventory);
					}
					//Fourth Row
					if(GUI.Button(new Rect(480 - SubtractionX, 292 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[12]], TooltipText[12]))) {
						SlotHandle(12, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(544 - SubtractionX, 292 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[13]], TooltipText[13]))) {
						SlotHandle(13, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(608 - SubtractionX, 292 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[14]], TooltipText[14]))) {
						SlotHandle(14, InventoryBar.Inventory);
					}
					if(GUI.Button(new Rect(672 - SubtractionX, 292 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)Items[15]], TooltipText[15]))) {
						SlotHandle(15, InventoryBar.Inventory);
					}
					//Equipping Row
					if(GUI.Button(new Rect(544 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)EquippedItems[0]], TooltipText[16]))) {
						print("The Target Is Moving The Item. I Repeat The Target Is Moving The Item");
						SlotHandle(0, InventoryBar.Equipped);
					}
					if(GUI.Button(new Rect(608 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)EquippedItems[1]], TooltipText[17]))) {
						if(Moving && curItemType == ItemType.Spell) {
							SlotHandle(1, InventoryBar.Equipped);
						}
						else if(!Moving) {
							SlotHandle(1, InventoryBar.Equipped);
						}
					}
					if(GUI.Button(new Rect(672 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)EquippedItems[2]], TooltipText[18]))) {
						if(Moving && curItemType == ItemType.Ring) {
							SlotHandle(2, InventoryBar.Equipped);
						}
						else if(!Moving) {
							SlotHandle(2, InventoryBar.Equipped);
						}
					}
					if(GUI.Button(new Rect(736 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)EquippedItems[3]], TooltipText[19]))) {
						if(Moving && curItemType == ItemType.Helmet) {
							SlotHandle(3, InventoryBar.Equipped);
						}
						else if(!Moving) {
							SlotHandle(3, InventoryBar.Equipped);
						}
					}
					if(GUI.Button(new Rect(800 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), new GUIContent(InventoryTextures[(int)EquippedItems[4]], TooltipText[20]))) {
						if(Moving && curItemType == ItemType.Chestplate) {
							SlotHandle(4, InventoryBar.Equipped);
						}
						else if(!Moving) {
							SlotHandle(4, InventoryBar.Equipped);
						}
					}

					GUI.Label(new Rect(780 - SubtractionX, 420 - SubtractionY, 128, 64), "Gold: " + Stats.Gold);

					if(EquippedItems[0] == Item.None) {
			 			GUI.DrawTexture(new Rect(544 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), EquippingEmptyTextures[0]);
					}
					if(EquippedItems[1] == Item.None) {
			 			GUI.DrawTexture(new Rect(608 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), EquippingEmptyTextures[1]);
					}
					if(EquippedItems[2] == Item.None) {
			 			GUI.DrawTexture(new Rect(672 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), EquippingEmptyTextures[2]);
					}
					if(EquippedItems[3] == Item.None) {
			 			GUI.DrawTexture(new Rect(736 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), EquippingEmptyTextures[3]);
					}
					if(EquippedItems[4] == Item.None) {
			 			GUI.DrawTexture(new Rect(800 - SubtractionX - 64 - 32, 420 - SubtractionY, 64, 64), EquippingEmptyTextures[4]);
					}

					//Tooltips
					if(!Moving) {
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
						GUI.Label(new Rect(Event.current.mousePosition.x + 20, Event.current.mousePosition.y, 1000, 1000), GUI.tooltip);
					}
					

					//Moving
					if(Moving) {
						GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 64, 64), movingTex);
					}
				}
			}
		}
	}	
}