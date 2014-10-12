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
using Holoville.HOEditorUtils;

namespace Scurge.Editor {
	public class ItemEditor : EditorWindow {

		public static EditorWindow window;

		public Inventory Inventory;
		public HeldItem CurrentHeldItem;
		public bool selected = false;
		public static Texture2D icon;

		public bool SelectingHeldItem = false;

		public Item itemsList;

		[MenuItem ("Tools/Scurge/Windows/Item Editor %#e")]
		static void Init () {
			window = EditorWindow.GetWindow(typeof (ItemEditor));
			icon = (Texture2D)Resources.Load("Inventory Icon", typeof(Texture2D));
			HOPanelUtils.SetWindowTitle(window, icon, "Item Editor");
		}

		void Update() {
			if(Inventory != null) {
				selected = true;
			}
			else {
				selected = false;
			}
			CurrentHeldItem = Inventory.heldItems[(int)itemsList];
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
							EditorGUI.indentLevel++;
							EditorGUILayout.LabelField("Health Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Health", Inventory.StatsAmounts[(int)itemsList]);
							EditorGUI.indentLevel--;
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Attack) {
							EditorGUI.indentLevel++;
							EditorGUILayout.LabelField("Attack Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Attack", Inventory.StatsAmounts[(int)itemsList]);
							EditorGUI.indentLevel--;
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Defense) {
							EditorGUI.indentLevel++;
							EditorGUILayout.LabelField("Defense Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Defense", Inventory.StatsAmounts[(int)itemsList]);
							EditorGUI.indentLevel--;
						}
						else if(Inventory.RingTypes[(int)itemsList] == Ring.Magic) {
							EditorGUI.indentLevel++;
							EditorGUILayout.LabelField("Magic Ring");
							Inventory.StatsAmounts[(int)itemsList] = EditorGUILayout.IntField("Magic", Inventory.StatsAmounts[(int)itemsList]);
							EditorGUI.indentLevel--;
						}
					}
					else if(Inventory.Types[(int)itemsList] == ItemType.Spell) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.spell.name = EditorGUILayout.TextField("Spell Name", CurrentHeldItem.spell.name);
						CurrentHeldItem.spell.spellType = (SpellType)EditorGUILayout.EnumPopup("Spell Type", CurrentHeldItem.spell.spellType);
						#region Spell Type Variables
						EditorGUI.indentLevel++;
						if(CurrentHeldItem.spell.spellType == SpellType.Heal) {
							CurrentHeldItem.spell.healAmount = EditorGUILayout.IntField("Healing Amount", CurrentHeldItem.spell.healAmount);
						}
						else if(CurrentHeldItem.spell.spellType == SpellType.Projectile) {
							CurrentHeldItem.spell.projectile = (GameObject)EditorGUILayout.ObjectField("Projectile GameObject", CurrentHeldItem.spell.projectile, typeof(GameObject), true);
						}
						else if(CurrentHeldItem.spell.spellType == SpellType.ExpandingSphere) {
							CurrentHeldItem.spell.hasParticle = true;
						
							EditorGUILayout.LabelField("Sphere Gets Assigned At Runtime");
							CurrentHeldItem.spell.radiusToExpand = EditorGUILayout.IntSlider("Max Size", CurrentHeldItem.spell.radiusToExpand, 0, 100);
							CurrentHeldItem.spell.deflectionDuration = EditorGUILayout.FloatField("Duration", CurrentHeldItem.spell.deflectionDuration);
						}
						else if(CurrentHeldItem.spell.spellType == SpellType.Nothing) {
							
						}
						EditorGUI.indentLevel--;
						#endregion
						CurrentHeldItem.spell.hasParticle = EditorGUILayout.Toggle("Has Particle", CurrentHeldItem.spell.hasParticle);
						if(CurrentHeldItem.spell.hasParticle) {
							EditorGUI.indentLevel++;
							CurrentHeldItem.spell.particle = (GameObject)EditorGUILayout.ObjectField("Particle Object", CurrentHeldItem.spell.particle, typeof(GameObject), true);
							CurrentHeldItem.spell.attachParticleToPlayer = EditorGUILayout.Toggle("Attach To Player", CurrentHeldItem.spell.attachParticleToPlayer);
							EditorGUI.indentLevel--;
						}
						CurrentHeldItem.spell.positionAdder = EditorGUILayout.Vector3Field("Particle Position Adder", CurrentHeldItem.spell.positionAdder);
						CurrentHeldItem.spell.ManaCost = EditorGUILayout.IntSlider("Mana Cost", CurrentHeldItem.spell.ManaCost, 0, 500);
						CurrentHeldItem.spell.sound = (AudioSource)EditorGUILayout.ObjectField("Spell Sound", CurrentHeldItem.spell.sound, typeof(AudioSource), true);
						EditorGUI.indentLevel--;
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
						Inventory.StatsAmounts.Add(0);
						Inventory.PotionTypes.Add(Potion.None);
						Inventory.spells.Add(new Spell());
						foreach(HealthVariables curHealthVariables in Inventory.enemyProximities) {
							curHealthVariables.HeldItems.Add(CurrentHeldItem);
						}
						Inventory.heldItems.Add(CurrentHeldItem);
					}
					if(GUILayout.Button("Delete Item")) {
						Inventory.ItemDescription.RemoveAt((int)itemsList);
						Inventory.InventoryTextures.RemoveAt((int)itemsList);
						Inventory.Types.RemoveAt((int)itemsList);
						Inventory.ItemObjects.RemoveAt((int)itemsList);
						Inventory.ThrownObjects.RemoveAt((int)itemsList);
						Inventory.RingTypes.RemoveAt((int)itemsList);
						Inventory.StatsAmounts.RemoveAt((int)itemsList);
						Inventory.PotionTypes.RemoveAt((int)itemsList);
						Inventory.spells.RemoveAt((int)itemsList);
						foreach(HealthVariables curHealthVariables in Inventory.enemyProximities) {
							curHealthVariables.HeldItems.RemoveAt((int)itemsList);
						}
						Inventory.heldItems.RemoveAt((int)itemsList);
					}
				}
				else {
					GUILayout.Space(40);
					CurrentHeldItem.ItemType = (Type)EditorGUILayout.EnumPopup("Item Type", CurrentHeldItem.ItemType);
					CurrentHeldItem.ItemSide = (Side)EditorGUILayout.EnumPopup("Button To Use", CurrentHeldItem.ItemSide);
					CurrentHeldItem.ItemCommand = (Command)EditorGUILayout.EnumPopup("Command To Call", CurrentHeldItem.ItemCommand);
					CurrentHeldItem.item= (Item)EditorGUILayout.EnumPopup("What Item Am I", CurrentHeldItem.item);
					CurrentHeldItem.Skin = (GUISkin)EditorGUILayout.ObjectField("GUI Skin", CurrentHeldItem.Skin, typeof(GUISkin), true);
					if(CurrentHeldItem.ItemCommand == Command.Melee) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.DamageMin = EditorGUILayout.IntField("Minimum Damage", CurrentHeldItem.DamageMin);
						CurrentHeldItem.DamageMax = EditorGUILayout.IntField("Maximum Damage", CurrentHeldItem.DamageMax);
						EditorGUI.indentLevel--;
					}
					else if(CurrentHeldItem.ItemCommand == Command.Throw) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.Projectile = (GameObject)EditorGUILayout.ObjectField("Object To Throw", CurrentHeldItem.Projectile, typeof(GameObject));
						EditorGUI.indentLevel--;
					}
					else if(CurrentHeldItem.ItemCommand == Command.Spell) {

					}
					CurrentHeldItem.DestroyOnUse = EditorGUILayout.Toggle("Destroy On Use", CurrentHeldItem.DestroyOnUse);
					if(CurrentHeldItem.DestroyOnUse) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.DestroySound = (AudioSource)EditorGUILayout.ObjectField("Sound To Play On Destroy", CurrentHeldItem.DestroySound, typeof(AudioSource));
						EditorGUI.indentLevel--;
					}
					CurrentHeldItem.LimitedUses = EditorGUILayout.Toggle("Limited Uses", CurrentHeldItem.LimitedUses);
					if(CurrentHeldItem.LimitedUses) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.Uses = EditorGUILayout.IntField("Uses Left", CurrentHeldItem.Uses);
						CurrentHeldItem.MaxUses = EditorGUILayout.IntField("Max Uses", CurrentHeldItem.MaxUses);
						EditorGUI.indentLevel--;
					}
					CurrentHeldItem.Cooldown = EditorGUILayout.Toggle("Has Cooldown", CurrentHeldItem.Cooldown);
					if(CurrentHeldItem.Cooldown) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.CooldownTime = EditorGUILayout.FloatField("Cooldown Timer", CurrentHeldItem.CooldownTime);
						EditorGUI.indentLevel--;
					}
					CurrentHeldItem.HasAnimation = EditorGUILayout.Toggle("Has Animation", CurrentHeldItem.HasAnimation);
					if(CurrentHeldItem.HasAnimation) {
						EditorGUI.indentLevel++;
						CurrentHeldItem.SwingAnimation = (Animation)EditorGUILayout.ObjectField("Animation Clip", CurrentHeldItem.SwingAnimation, typeof(Animation));
						EditorGUI.indentLevel--;
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
	    		this.Repaint();
	    	}
	}
}