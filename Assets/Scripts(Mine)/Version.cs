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
		public string CodeName;
		public WWW VersionCheckWWW;
		public bool Enabled = true;

		IEnumerator Start() {
		        	VersionCheckWWW = new WWW("http://zaegames-drabweb.rhcloud.com/SotS_2/version.txt");
		        	yield return VersionCheckWWW;
		}

		void OnGUI() {
			GUI.skin = Skin;

			if(Enabled) {
				if(VersionCheckWWW.text != DevelopmentStage + " " + GameVersion) {
					if(GUI.Button(new Rect(rect.x - 333, rect.y - 23, 500, rect.height), "<color=#ff0000ff>New Version Avaliable! Version " + VersionCheckWWW.text + "</color>", "Button No Texture")) {
						Application.OpenURL("http://zaegames-drabweb.rhcloud.com/SotS_2/");
					}
				}
				GUI.Label(rect, DevelopmentStage + " " + GameVersion);
				GUI.Label(new Rect(rect.x - 65, rect.y + 20, rect.width, rect.height), CodeName);
			}
		}
	}
}