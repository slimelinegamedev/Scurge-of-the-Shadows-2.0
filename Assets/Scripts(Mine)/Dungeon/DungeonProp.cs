using UnityEngine;
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

namespace Scurge.Environment {
	public class DungeonProp : MonoBehaviour {

		#region vars
		public Dungeon Dungeon;
		public GameObject propObj;
		public PropSize size;
		public bool Hanging = false;
		#endregion
	}
}