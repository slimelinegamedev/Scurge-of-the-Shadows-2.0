using UnityEngine;
using System.Collections;
using Scurge;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

namespace Scurge.Player {
	public class ItemList : ScriptableObject {
		public List<ItemDefinition> items;
	}
}