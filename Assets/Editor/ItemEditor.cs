using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

namespace Scurge.Editor {
	public class ItemEditor : EditorWindow {

		public Inventory Inventory;
		public HeldItem CurrentHeldItem;
		public bool selected = false;

		public bool SelectingHeldItem = false;

		public Item itemsList;

		[MenuItem ("Window/Item Editor %#e")]
		static void Init () {
			EditorWindow window = EditorWindow.GetWindow(typeof (ItemEditor));
			window.title = "Item Creator";
		}

		void Update() {
			if(Inventory != null) {
				selected = true;
			}
			else {
				selected = false;
			}
		}

		void OnGUI() {
			if(selected) {
				GUILayout.BeginHorizontal();
					if(SelectingHeldItem) {
						GUILayout.Label("Held Item Editor For SotS 2.0");
					}
					else {
						GUILayout.Label("Item Editor For SotS 2.0");
					}
					if(SelectingHeldItem == true) {
						if(GUILayout.Button("Inventory Options")) {
							SelectingHeldItem = false;
						}
					}
					else if(SelectingHeldItem == false) {
						if(GUILayout.Button("Held Item Options")) {
							SelectingHeldItem = true;
						}
					}
					GUILayout.Space(5);
					itemsList = (Item)EditorGUILayout.EnumPopup("Item Editing", itemsList);
				GUILayout.EndHorizontal();
				if(!SelectingHeldItem) {
					GUILayout.Space(40);
					GUILayout.Label("Item tooltip");
					GUILayout.BeginHorizontal();
						Inventory.ItemDescription[(int)itemsList] = EditorGUILayout.TextArea(Inventory.ItemDescription[(int)itemsList], GUILayout.Height(100));
						Inventory.InventoryTextures[(int)itemsList] = (Texture2D)EditorGUILayout.ObjectField("Item Texture In Inventory", Inventory.InventoryTextures[(int)itemsList], typeof(Texture2D));
					GUILayout.EndHorizontal();
					Inventory.Types[(int)itemsList] = (ItemType)EditorGUILayout.EnumPopup("Item Type", Inventory.Types[(int)itemsList]);
					if(Inventory.Types[(int)itemsList] == ItemType.Ring) {
						Inventory.RingTypes[(int)itemsList] = (Ring)EditorGUILayout.EnumPopup("Ring Type", Inventory.RingTypes[(int)itemsList]);
						if(Inventory.RingTypes[(int)itemsList] == Ring.None) {

						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Health) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Health Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Health", Inventory.StatsAmounts[(int)itemsList]);
							GUILayout.EndHorizontal();
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Attack) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Attack Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Attack", Inventory.StatsAmounts[(int)itemsList]);
							GUILayout.EndHorizontal();
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Defense) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Defense Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Defense", Inventory.StatsAmounts[(int)itemsList]);
							GUILayout.EndHorizontal();
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Magic) {
							GUILayout.BeginHorizontal();
							GUILayout.Label("Magic Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Magic", Inventory.StatsAmounts[(int)itemsList]);
							GUILayout.EndHorizontal();
						}
					}
					else if(Inventory.Types[(int)itemsList] == ItemType.Spell) {
						
					}
					else if(Inventory.Types[(int)itemsList] == ItemType.Helmet) {
						
					}
					else if(Inventory.Types[(int)itemsList] == ItemType.Chestplate) {
						
					}
					else if(Inventory.Types[(int)itemsList] == ItemType.Potion) {
						
					}
					Inventory.ItemObjects[(int)itemsList] = (GameObject)EditorGUILayout.ObjectField("Item Held Object", Inventory.ItemObjects[(int)itemsList], typeof(GameObject));
					Inventory.ThrownObjects[(int)itemsList] = (GameObject)EditorGUILayout.ObjectField("Item Thrown Object", Inventory.ThrownObjects[(int)itemsList], typeof(GameObject));
					GUILayout.Space(10);
					if(GUILayout.Button("New Item")) {
						Inventory.ItemDescription.Add(null);
						Inventory.InventoryTextures.Add(null);
						Inventory.Types.Add(ItemType.None);
						Inventory.ItemObjects.Add(null);
						Inventory.ThrownObjects.Add(null);
						Inventory.RingTypes.Add(Ring.None);
					}
					if(GUILayout.Button("Delete Item")) {
						Inventory.ItemDescription.RemoveAt((int)itemsList);
						Inventory.InventoryTextures.RemoveAt((int)itemsList);
						Inventory.Types.RemoveAt((int)itemsList);
						Inventory.ItemObjects.RemoveAt((int)itemsList);
						Inventory.ThrownObjects.RemoveAt((int)itemsList);
						Inventory.RingTypes.RemoveAt((int)itemsList);
					}
				}
				else {
					GUILayout.Space(40);
					CurrentHeldItem = Inventory.ItemObjects[(int)itemsList].GetComponent<HeldItem>();
					CurrentHeldItem.ItemType = (Type)EditorGUILayout.EnumPopup("Item Type", CurrentHeldItem.ItemType);
					CurrentHeldItem.ItemSide = (Side)EditorGUILayout.EnumPopup("Button To Use", CurrentHeldItem.ItemSide);
					CurrentHeldItem.ItemCommand = (Command)EditorGUILayout.EnumPopup("Command To Call", CurrentHeldItem.ItemCommand);
					CurrentHeldItem.item= (Item)EditorGUILayout.EnumPopup("What Item Am I", CurrentHeldItem.item);
					if(CurrentHeldItem.ItemCommand == Command.Melee) {
						CurrentHeldItem.DamageMin = EditorGUILayout.IntField("Minimum Damage", CurrentHeldItem.DamageMin);
						CurrentHeldItem.DamageMax = EditorGUILayout.IntField("Maximum Damage", CurrentHeldItem.DamageMax);
					}
					if(CurrentHeldItem.ItemCommand == Command.Summon) {

					}
					else if(CurrentHeldItem.ItemCommand == Command.Projectile) {
						CurrentHeldItem.Projectile = (GameObject)EditorGUILayout.ObjectField("Projectile Object", CurrentHeldItem.Projectile, typeof(GameObject));
					}
					CurrentHeldItem.DestroyOnUse = EditorGUILayout.Toggle("Destroy On Use", CurrentHeldItem.DestroyOnUse);
					if(CurrentHeldItem.DestroyOnUse) {
						CurrentHeldItem.DestroySound = (AudioSource)EditorGUILayout.ObjectField("Sound To Play On Destroy", CurrentHeldItem.DestroySound, typeof(AudioSource));
					}
					CurrentHeldItem.LimitedUses = EditorGUILayout.Toggle("Limited Uses", CurrentHeldItem.LimitedUses);
					if(CurrentHeldItem.LimitedUses) {
						CurrentHeldItem.Uses = EditorGUILayout.IntField("Uses Left", CurrentHeldItem.Uses);
						CurrentHeldItem.MaxUses = EditorGUILayout.IntField("Max Uses", CurrentHeldItem.MaxUses);
					}
					CurrentHeldItem.Cooldown = EditorGUILayout.Toggle("Has Cooldown", CurrentHeldItem.Cooldown);
					if(CurrentHeldItem.Cooldown) {
						CurrentHeldItem.CooldownTime = EditorGUILayout.FloatField("Cooldown Timer", CurrentHeldItem.CooldownTime);
					}
					CurrentHeldItem.HasAnimation = EditorGUILayout.Toggle("Has Animation", CurrentHeldItem.HasAnimation);
					if(CurrentHeldItem.HasAnimation) {
						CurrentHeldItem.SwingAnimation = (Animation)EditorGUILayout.ObjectField("Animation Clip", CurrentHeldItem.SwingAnimation, typeof(Animation));
					}
					CurrentHeldItem.Sound = (AudioSource)EditorGUILayout.ObjectField("Sound To Play", CurrentHeldItem.Sound, typeof(AudioSource));
				}
			}
			else if(!selected) {
				Inventory = (Inventory)EditorGUILayout.ObjectField(Inventory, typeof(Inventory), true);
				GUILayout.Space(50);
				EditorGUILayout.LabelField("Please select an inventory script. If you cant find one, open the scene called main and it should be on the player");
			}
			if(Inventory != null) {
				EditorUtility.SetDirty(Inventory.gameObject);
			}
		}
		void OnInspectorUpdate() {
	    		Repaint();
	    	}
	}
}