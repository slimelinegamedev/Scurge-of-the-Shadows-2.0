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
using TeamUtility.IO;

namespace Scurge.Environment {
	public class Gold : MonoBehaviour {

		public Stats Stats;
		public AudioSource Sound;

		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				Stats.GiveGold(1);
				Sound.Play();
				Destroy(gameObject);
			}
		}
	}
}