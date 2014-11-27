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
using TeamUtility.IO;
using System.Globalization;

namespace Scurge.Player {
	#region Enums
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
		RingRed = 5,
		DaggerThrowing = 6,
		SpellRedGlow = 7,
		SpellRepair = 8,
		SpellDeflection = 9
	}

	public enum Ring {
		None = 0,
		Attack = 1,
		Defense = 2,
		Magic = 3,
		Health = 4
	}

	public enum SpellType {
		Projectile = 0,
		Heal = 1,
		ExpandingSphere = 2,
		Nothing = 3
	}
	#endregion

	#region Structs
	[System.Serializable]
	public struct Spell {
		public string name;
		public bool hasParticle;
		public GameObject particle;
		public AudioSource sound;
		public SpellType spellType;
		#region Spell Type Variables, Most Arent Used
		public int healAmount;

		public GameObject projectile;

		public SphereCollider sphereToExpand;
		public int radiusToExpand;
		public float deflectionDuration;
		#endregion
		public bool attachParticleToPlayer;
		public Vector3 positionAdder;
		public int ManaCost;
	}
	#endregion

	public class Inventory : MonoBehaviour {
		#region Variables
		public Stats Stats;
		public Objects Objects;
		public Pause Pause;

		public bool Visible;
		public bool InventoryOpen;
		public Texture2D Crosshair;
		public float SubtractionX;
		public float SubtractionY;
		public Rect BackgroundRect;
		public Disable Disable;
		public int FirstOpenSlot;
		public bool Full = false;
		public int ActiveItem;
		public GUISkin Skin;
		public HeldItem curStaff;

		//UI Related
		[Header("UI Related")]
		public List<Button> ItemSlots;
		public List<Button> EquippedItemSlots;
		public List<Image> ItemSlotsImages;
		public List<Image> EquippedItemSlotsImages;
		public List<Sprite> InventorySprites;
		//USed to move the DragImage
		public List<Vector3> SlotPositions;
		public Text DescriptionLabel;
		public Text DescriptionSubtitle;
		public Image DragImage;
		[Range(0, 20)]
		public int CurrentSelectedSlot;

		[Header("Dragging Things")]
		//Current item being moved
		public Item movingItem;
		//Last moved item
		public Item lastMovedItem;
		//Current type of item
		public ItemType curItemType;
		//Last type of item
		public ItemType lastItemType;
		//Is moving item?
		public bool Moving;

		[Header("Technical Things")]
		public List<Item> Items;
		public List<Item> EquippedItems;
		public List<HeldItem> heldItems;
		public List<ItemType> Types;
		public List<string> ItemDescription;
		public List<GameObject> ItemObjects;
		public List<GameObject> ThrownObjects;
		public List<Texture2D> InventoryTextures;
		public List<Texture2D> EquippingEmptyTextures;
		//Variables for other item types
		public List<Ring> RingTypes;
		public List<int> StatsAmounts;
		public List<Potion> PotionTypes;
		public List<Spell> spells;
		//End
		public List<string> TooltipText;
		private bool HasFound = false;
		//All enemys proximity detectors
		public List<HealthVariables> enemyProximities;
		#endregion

		#region Deprecated
		/*public void Move(Item curItem, Texture2D curItemTex, int slot, InventoryBar bar, ItemType type) {
			print("Moving");
			if(bar == InventoryBar.Inventory && Moving == false) {
				print("Moving : Made It Past Checkup");
				lastMovedItem = curItem;
				Items [slot] = Item.None;
				Moving = true;
				movingTex = curItemTex;
				movingItem = curItem;
				curItemType = type;
			}
			else if(bar == InventoryBar.Equipped && !Moving) {
				print("InventoryBar.Equipped");
				print("Moving : Made It Past Checkup");
				lastMovedItem = curItem;
				EquippedItems [slot] = Item.None;
				Moving = true;
				movingTex = curItemTex;
				movingItem = curItem;
				curItemType = type;
			}
		}
		public void Place(int slot, InventoryBar bar) {
			if(bar == InventoryBar.Inventory) {
				print("Placing InventoryBar.Inventory[" + slot.ToString() + "]");
				lastMovedItem = Item.None;
				Items [slot] = movingItem;
				Moving = false;
				movingItem = Item.None;
				movingTex = null;
			}
			else if(bar == InventoryBar.Equipped) {
				print("Placing InventoryBar.Equipped[" + slot.ToString() + "]");
				lastMovedItem = Item.None;
				EquippedItems [slot] = movingItem;
				Moving = false;
				movingItem = Item.None;
				movingTex = null;
			}
		}

		public void Swap(int slot, InventoryBar bar) {
			if(bar == InventoryBar.Inventory) {
				movingItem = Items [slot];
				movingTex = InventoryTextures [(int)Items [slot]];
				Items [slot] = lastMovedItem;
			}
			else if(bar == InventoryBar.Equipped) {
				print("InventoryBar.Equipped");
				movingItem = EquippedItems [slot];
				movingTex = InventoryTextures [(int)EquippedItems [slot]];
				EquippedItems [slot] = lastMovedItem;
			}
		}

		public void SlotHandle(int slot, InventoryBar barbar) {
			if(barbar == InventoryBar.Inventory) {
				print("Picked up on InventoryBar.Inventory");
				if(Moving == false && Items [slot] != Item.None) {
					Move(Items [slot], InventoryTextures [(int)Items [slot]], slot, InventoryBar.Inventory, Types [(int)Items [slot]]);
				}
				else if(Moving && Items [slot] == Item.None) {
					print("Placed Item On InventoryBar.Inventory");
					Place(slot, InventoryBar.Inventory);
				}
				else if(Moving && Items [0] != Item.None) {
					print("Swapped Item For First Time On InventoryBar.Inventory");
					Swap(slot, InventoryBar.Inventory);
				}
			}
			else if(barbar == InventoryBar.Equipped) {
				print("Picked up on InventoryBar.Equipped");
				if(!Moving && EquippedItems [slot] != Item.None) {
					Move(EquippedItems [slot], InventoryTextures [(int)EquippedItems [slot]], slot, InventoryBar.Equipped, Types [(int)EquippedItems [slot]]);
				}
				else if(Moving && EquippedItems [slot] == Item.None) {
					print("Placed Item On InventoryBar.Equipped");
					Place(slot, InventoryBar.Equipped);
				}
				else if(Moving && EquippedItems [slot] != Item.None) {
					print("Swapped Item For First Time On InventoryBar.Equipped");
					Swap(slot, InventoryBar.Equipped);
				}
			}
		}*/
		#endregion

		#region Functions
		public bool HasEquipped(Item item) {
			if(EquippedItems [0] == item) {
				return true;
			}
			else {
				return false;
			}
		}
		public bool NoItemAtSlot(int slot) {
			if(slot < 16) {
				if(Items [slot] == Item.None) {
					return true;
				}
			}
			else {
				if(EquippedItems [slot] == Item.None) {
					return true;
				}
			}
			return false;
		}
		public void Give(Item item, int slot, InventoryBar bar) {
			if(!Full) {
				if(bar == InventoryBar.Inventory) {
					Items [slot] = item;
				}
				else if(bar == InventoryBar.Equipped) {
					EquippedItems [slot] = item;
				}
			}
		}

		public void Delete(int slot, InventoryBar bar) {
			if(bar == InventoryBar.Equipped) {
				EquippedItems [slot] = Item.None;
			}
			else if(bar == InventoryBar.Inventory) {
				Items [slot] = Item.None;
			}
		}

		public void EquipItems() {
			ItemObjects [(int)EquippedItems [0]].SetActive(true);

			ActiveItem = (int)EquippedItems [0];

			for(int allItemsCycle = 0; allItemsCycle < ItemObjects.Count; allItemsCycle++) {
				if(allItemsCycle != ActiveItem) {
					ItemObjects [allItemsCycle].SetActive(false);
				}
			}
		}

		public void ThrowItem(Item thrower) {
			var ThrownItem = (GameObject)Instantiate(ThrownObjects [(int)thrower], transform.position, transform.rotation);
			ThrownItem.transform.rigidbody.AddForce(Objects.Camera.transform.TransformDirection(Vector3.forward) * 500);
		}
		public void ThrowMovingItem() {
			if(Moving) {
				//Set the item to throw
				var ThrownItem = (GameObject)Instantiate(ThrownObjects [(int)movingItem], transform.position, transform.rotation);

				//Reset drag item
				movingItem = Item.None;
				DragImage.gameObject.SetActive(false);
				Moving = false;

				//Throw
				ThrownItem.transform.rigidbody.AddForce(Objects.Camera.transform.TransformDirection(Vector3.forward) * 500);
			}
			else {
				print("Nothing moving to throw");
			}
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

		public void ResetAllInventoryUI() {
			foreach(Image curItemImage in ItemSlotsImages) {
				curItemImage.color = new Color(1, 1, 1, 0);
			}
			foreach(Image curEquippingItemImage in EquippedItemSlotsImages) {
				curEquippingItemImage.color = new Color(1, 1, 1, 0);
			}
		}
		public void SetItemSlotTextures() {
			for(int iterateInventoryImages = 0; iterateInventoryImages < ItemSlotsImages.Count; iterateInventoryImages++) {
				if(Items[iterateInventoryImages] != Item.None) {
					//Something there, make visible and set image
					ItemSlotsImages[iterateInventoryImages].color = new Color(1, 1, 1, 1);
					ItemSlotsImages[iterateInventoryImages].overrideSprite = InventorySprites[(int)Items[iterateInventoryImages]];
				}
				else {
					//Nothing there, make invisible
					ItemSlotsImages[iterateInventoryImages].color = new Color(1, 1, 1, 0);
				}
			}
			for(int iterateInventoryEquippingImages = 0; iterateInventoryEquippingImages < EquippedItemSlotsImages.Count; iterateInventoryEquippingImages++) {
				if(EquippedItems[iterateInventoryEquippingImages] != Item.None) {
					//Something there, make visible and set image
					EquippedItemSlotsImages[iterateInventoryEquippingImages].color = new Color(1, 1, 1, 1);
					EquippedItemSlotsImages[iterateInventoryEquippingImages].overrideSprite = InventorySprites[(int)EquippedItems[iterateInventoryEquippingImages]];
				}
				else {
					//Nothing there, make invisible
					EquippedItemSlotsImages[iterateInventoryEquippingImages].color = new Color(1, 1, 1, 0);
				}
			}
		}
		public void SetTooltip() {
			//Not moving item
			if(!Moving) {
				//Split string by lines
				string[] Tooltip = TooltipText [CurrentSelectedSlot].Split('\n');
				//Get first in array for header
				DescriptionLabel.text = Tooltip[0];
				//Use the rest for the subtitle
				foreach(string currentTooltipText in Tooltip) {
					DescriptionSubtitle.text = currentTooltipText;
				}
			}
			else {
				string[] Tooltip = ItemDescription [(int)movingItem].Split('\n');
				//Get first in array for header
				DescriptionLabel.text = Tooltip[0];
				//Use the rest for the subtitle
				foreach(string currentTooltipText in Tooltip) {
					DescriptionSubtitle.text = currentTooltipText;
				}
			}
		}
		public void SetSelectedSlot(int slot) {
			CurrentSelectedSlot = slot;
		}
		//TODO: Allow swapping of items when moving
		//TODO: Set NPC GUI to use 4.6 UI
		public void HandleDrag(int slot) {
			if(!Moving) {
				//Not moving
				if(slot < 16) {
					//Were in the regular inventory
					if(Items [slot] != Item.None) {
						print("Something In Slot!");
						//Item at slot is not nothing

						//movingItem is equal to that of the item in the selected slot
						movingItem = Items [slot];

						//Set sprite to correct image texture
						DragImage.overrideSprite = InventorySprites [(int)Items [slot]];

						//Set the item type
						curItemType = Types[(int)Items[slot]];

						//Set the slot item to none, wre dragging
						Items [slot] = Item.None;

						//enable the drag image
						DragImage.gameObject.SetActive(true);  

						//Set moving to true, were dragging now
						Moving = true;
					}
					else {
						print("Nothing In Slot...");
					}
				}
				else {
					//Were in the equipping inventory
					if(EquippedItems [slot - 16] != Item.None) {
						print("Something In Slot!");
						//Item at slot is not nothing

						//movingItem is equal to that of the item in the selected slot
						movingItem = EquippedItems [slot - 16];

						//Set sprite to correct image texture
						DragImage.overrideSprite = InventorySprites [(int)EquippedItems [slot - 16]];

						//Set the item type
						curItemType = Types[(int)EquippedItems[slot - 16]];

						//Set the slot item to none, wre dragging
						EquippedItems [slot - 16] = Item.None;

						//enable the drag image
						DragImage.gameObject.SetActive(true);  

						//Set moving to true, were dragging now
						Moving = true;
					}
					else {
						print("Nothing In Slot...");
					}
				}
			}
			else if(Moving) {
				if(slot < 16) {
					if(Items [slot] == Item.None) {
						lastMovedItem = Items [slot];
						Items [slot] = movingItem;
						movingItem = lastMovedItem;
						DragImage.gameObject.SetActive(false);
						curItemType = ItemType.None;
						Moving = false;

						print("Dropped Item Down!");
					}
					else {
						//Swappping
						print("Swapping In Slot " + (slot).ToString());
						lastMovedItem = Items [slot];
						Items [slot] = movingItem;
						movingItem = lastMovedItem;
						DragImage.overrideSprite = InventorySprites [(int)movingItem];
					}
				}
				else {
					//Checking if can go into equipping slot (Check type)
					print("Slot - 16 is " + (slot - 16).ToString());
					if(slot - 16 == 0) {
						print("Using Equipping Slot 0");
						if(EquippedItems [slot - 16] != Item.None) {
							if(Moving) {
								//Moving Item
								if(EquippedItems[slot - 16] != Item.None) {
									//Swapping
									print("Swapping In Slot " + (slot - 16).ToString());
									lastMovedItem = EquippedItems [slot - 16];
									EquippedItems [slot - 16] = movingItem;
									movingItem = lastMovedItem;
									DragImage.overrideSprite = InventorySprites [(int)movingItem];
								}
							}
							else {
								//Doesnt Have Anything Moving
								print("Nothing Is Moving!");
							}
						}
						else {
							//Nothing here, regular place
							print("Nothing in This Equipping Slot, Placing!");

							lastMovedItem = EquippedItems [slot - 16];
							EquippedItems [slot - 16] = movingItem;
							movingItem = lastMovedItem;
							DragImage.gameObject.SetActive(false);
							curItemType = ItemType.None;
							Moving = false;

							print("Dropped Item Down!");
						}
					}
					else if(slot - 16 == 1) {
						print("Using Equipping Slot 1");
						if(curItemType == ItemType.Spell) {
							if(EquippedItems [slot - 16] != Item.None) {
								if(Moving) {
									//Moving Item
									if(EquippedItems[slot - 16] != Item.None) {
										//Swapping
										print("Swapping In Slot " + (slot - 16).ToString());
										lastMovedItem = EquippedItems [slot - 16];
										EquippedItems [slot - 16] = movingItem;
										movingItem = lastMovedItem;
										DragImage.overrideSprite = InventorySprites [(int)movingItem];
									}
								}
								else {
									//Doesnt Have Anything Moving
									print("Nothing Is Moving!");
								}
							}
							else if(EquippedItems [slot - 16] == Item.None) {
								//Nothing here, regular place
								print("Nothing in This Equipping Slot, Placing!");

								lastMovedItem = EquippedItems [slot - 16];
								EquippedItems [slot - 16] = movingItem;
								movingItem = lastMovedItem;
								DragImage.gameObject.SetActive(false);
								curItemType = ItemType.None;
								Moving = false;

								print("Dropped Item Down!");
							}
						}
						else {
							//Not right type
							print("Not Right Type");
						}
					}
					if(slot - 16 == 2) {
						print("Using Equipping Slot 2");
						if(curItemType == ItemType.Ring) {
							if(EquippedItems [slot - 16] != Item.None) {
								if(Moving) {
									//Moving Item
									if(EquippedItems[slot - 16] != Item.None) {
										//Swapping
										print("Swapping In Slot " + (slot - 16).ToString());
										lastMovedItem = EquippedItems [slot - 16];
										EquippedItems [slot - 16] = movingItem;
										movingItem = lastMovedItem;
										DragImage.overrideSprite = InventorySprites [(int)movingItem];
									}
								}
								else {
									//Doesnt Have Anything Moving
									print("Nothing Is Moving!");
								}
							}
							else if(EquippedItems [slot - 16] == Item.None) {
								//Nothing here, regular place
								print("Nothing in This Equipping Slot, Placing!");

								lastMovedItem = EquippedItems [slot - 16];
								EquippedItems [slot - 16] = movingItem;
								movingItem = lastMovedItem;
								DragImage.gameObject.SetActive(false);
								curItemType = ItemType.None;
								Moving = false;

								print("Dropped Item Down!");
							}
						}
						else {
							//Not right type
							print("Not Right Type");
						}
					}
					if(slot - 16 == 3) {
						print("Using Equipping Slot 3");
						if(curItemType == ItemType.Helmet) {
							if(EquippedItems [slot - 16] != Item.None) {
								if(Moving) {
									//Moving Item
									if(EquippedItems[slot - 16] != Item.None) {
										//Swapping
										print("Swapping In Slot " + (slot - 16).ToString());
										lastMovedItem = EquippedItems [slot - 16];
										EquippedItems [slot - 16] = movingItem;
										movingItem = lastMovedItem;
										DragImage.overrideSprite = InventorySprites [(int)movingItem];
									}
								}
								else {
									//Doesnt Have Anything Moving
									print("Nothing Is Moving!");
								}
							}
							else if(EquippedItems [slot - 16] == Item.None) {
								//Nothing here, regular place
								print("Nothing in This Equipping Slot, Placing!");

								lastMovedItem = EquippedItems [slot - 16];
								EquippedItems [slot - 16] = movingItem;
								movingItem = lastMovedItem;
								DragImage.gameObject.SetActive(false);
								curItemType = ItemType.None;
								Moving = false;

								print("Dropped Item Down!");
							}
						}
						else {
							//Not right type
							print("Not Right Type");
						}
					}
					if(slot - 16 == 4) {
						print("Using Equipping Slot 4");
						if(curItemType == ItemType.Chestplate) {
							if(EquippedItems [slot - 16] != Item.None) {
								if(Moving) {
									//Moving Item
									if(EquippedItems[slot - 16] != Item.None) {
										//Swapping
										print("Swapping In Slot " + (slot - 16).ToString());
										lastMovedItem = EquippedItems [slot - 16];
										EquippedItems [slot - 16] = movingItem;
										movingItem = lastMovedItem;
										DragImage.overrideSprite = InventorySprites [(int)movingItem];
									}
								}
								else {
									//Doesnt Have Anything Moving
									print("Nothing Is Moving!");
								}
							}
							else if(EquippedItems [slot - 16] == Item.None) {
								//Nothing here, regular place
								print("Nothing in This Equipping Slot, Placing!");

								lastMovedItem = EquippedItems [slot - 16];
								EquippedItems [slot - 16] = movingItem;
								movingItem = lastMovedItem;
								DragImage.gameObject.SetActive(false);
								curItemType = ItemType.None;
								Moving = false;

								print("Dropped Item Down!");
							}
						}
						else {
							//Not right type
							print("Not Right Type");
						}
					}
				}
			}
		}

		public void ApplyStats() {
			//Ring stats application
			//Temporary reset method
			for(int allRings = 0; allRings < RingTypes.Count; allRings++) {
				if(RingTypes [allRings] != Ring.None) {
					if(RingTypes [allRings] == Ring.Attack) {
						if(EquippedItems [2] == (Item)allRings) {
							Stats.Attack = StatsAmounts [allRings];
						}
						else {
							Stats.Attack = 0;
						}
					}
					if(RingTypes [allRings] == Ring.Defense) {
						if(EquippedItems [2] == (Item)allRings) {
							Stats.Defense = StatsAmounts [allRings];
						}
						else {
							Stats.Defense = 0;
						}
					}
					if(RingTypes [allRings] == Ring.Magic) {
						if(EquippedItems [2] == (Item)allRings) {

						}
					}
					if(RingTypes [allRings] == Ring.Health) {
						if(EquippedItems [2] == (Item)allRings) {
							Stats.MaxHealth = StatsAmounts [allRings];
							if(Stats.Health == Stats.MaxHealth) {
								Stats.Health = StatsAmounts [allRings];
							}
						}
						else {
							Stats.MaxHealth = 10;
						}
					}
				}
			}
		}
		#endregion

		#region Methods
		void Start() {
			curStaff.ChangeSpell(heldItems[(int)EquippedItems[1]].spell);
			DragImage.gameObject.SetActive(false);
		}

		void Update() {
			if(!InventoryOpen) {
				Screen.showCursor = false;
				Screen.lockCursor = true;
			}
			else {
				Screen.showCursor = true;
				Screen.lockCursor = false;
				if(EquippedItems[1] == Item.None) {
					curStaff.ChangeSpell(new Spell());
				}
				if(EquippedItems[1] != Item.None) {
					curStaff.ChangeSpell(heldItems[(int)EquippedItems[1]].spell);
				}
			}
			if(Moving) {
				DragImage.transform.localPosition = SlotPositions [CurrentSelectedSlot];
				if(cInput.GetKeyDown("Attack/Fire 2")) {
					ThrowMovingItem();
				}
			}
			FindSlot();
			EquipItems();
			ApplyStats();
			SetItemSlotTextures();
			SetTooltip();

			if(cInput.GetKeyDown("Inventory") && !Moving && !Pause.Open) {
				InventoryOpen = !InventoryOpen;
				if(InventoryOpen) {
					Screen.showCursor = true;
					Screen.lockCursor = false;
					Disable.DisableObj(false, false);
					print("Enabling(Showing) Inventory");
					Objects.UIInventory.SetActive(true);
				}
				else if(!InventoryOpen) {
					Screen.showCursor = false;
					Screen.lockCursor = true;
					Disable.EnableObj(false, false);
					print("Disabling(Hiding) Inventory");
					Objects.UIInventory.SetActive(false);
				}
			}
			//Give item tooltips
			TooltipText [0] = ItemDescription [(int)Items [0]];
			TooltipText [1] = ItemDescription [(int)Items [1]];
			TooltipText [2] = ItemDescription [(int)Items [2]];
			TooltipText [3] = ItemDescription [(int)Items [3]];
			TooltipText [4] = ItemDescription [(int)Items [4]];
			TooltipText [5] = ItemDescription [(int)Items [5]];
			TooltipText [6] = ItemDescription [(int)Items [6]];
			TooltipText [7] = ItemDescription [(int)Items [7]];
			TooltipText [8] = ItemDescription [(int)Items [8]];
			TooltipText [9] = ItemDescription [(int)Items [9]];
			TooltipText [10] = ItemDescription [(int)Items [10]];
			TooltipText [11] = ItemDescription [(int)Items [11]];
			TooltipText [12] = ItemDescription [(int)Items [12]];
			TooltipText [13] = ItemDescription [(int)Items [13]];
			TooltipText [14] = ItemDescription [(int)Items [14]];
			TooltipText [15] = ItemDescription [(int)Items [15]];
			//Equipped
			TooltipText [16] = ItemDescription [(int)EquippedItems [0]];
			TooltipText [17] = ItemDescription [(int)EquippedItems [1]];
			TooltipText [18] = ItemDescription [(int)EquippedItems [2]];
			TooltipText [19] = ItemDescription [(int)EquippedItems [3]];
			TooltipText [20] = ItemDescription [(int)EquippedItems [4]];
		}
		#endregion
	}	
}