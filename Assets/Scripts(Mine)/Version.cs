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

namespace Scurge.Util {
	public class Version : MonoBehaviour {

		public GUISkin Skin;

		public Rect rect;
		public string DevelopmentStage;
		public string GameVersion;
		public bool Enabled = true;

		void OnGUI() {

			GUI.skin = Skin;

			if(Enabled) {
				GUI.Label(rect, DevelopmentStage + " " + GameVersion);
			}
		}
	}
}