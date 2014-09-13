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

namespace Scurge.Util {
	public class DisableOnPlay : MonoBehaviour {
		public int WaitTime = 0;
		IEnumerator Start() {
			yield return new WaitForSeconds(WaitTime);
			gameObject.SetActive(false);
		}
	}
}