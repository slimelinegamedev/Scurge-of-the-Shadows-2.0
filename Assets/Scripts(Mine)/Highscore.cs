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

namespace Scurge.Scoreboard {
	public class Highscore : MonoBehaviour {

		public List<string> Scores;

		public void add(string playerName, int floor, int gold, string killedBy) {
			Scores.Add(playerName + " " + killedBy + " with " + gold + " gold on floor " + floor);
		}
	}
}