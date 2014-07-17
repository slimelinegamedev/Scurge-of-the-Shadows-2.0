using UnityEngine;
using System.Collections;
using Scurge;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

namespace Scurge {
	[System.Serializable]
	public class ItemDefinition {
		public string name;
		public string description;
		public Texture2D texture;
		public ItemType type;
		public Command command;
		public bool destroyOnUse = false;
		public GameObject held;
		public GameObject thrown;
		public int index;

		public int MinimumDamage;
		public int MaximumDamage;
		public int Defense;
		public Potion potionType; 
	}
}