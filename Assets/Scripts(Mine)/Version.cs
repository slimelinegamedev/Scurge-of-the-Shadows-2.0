using UnityEngine;
using UnityEngine.UI;
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

		public Button DownloadButton;
		public Text VersionText;
		public Text CodeNameText;

		IEnumerator Start() {
		        	VersionCheckWWW = new WWW("http://zaegames-drabweb.rhcloud.com/sots/version.txt");
		        	yield return VersionCheckWWW;
		}

		void Update() {
			if(Enabled) {
				if(VersionCheckWWW.text != DevelopmentStage + " " + GameVersion) {
					/*if(GUI.Button(new Rect(rect.x - 333, rect.y - 23, 500, rect.height), "<color=#ff0000ff>New Version Avaliable! Version " + VersionCheckWWW.text + "</color>", "Button No Texture")) {
						Application.OpenURL("http://zaegames-drabweb.rhcloud.com/SotS_2/");
					}*/
					DownloadButton.GetComponentInChildren<Text>().text = "<color=#ff0000ff>New Version Avaliable! Version " + VersionCheckWWW.text + "</color>";
					DownloadButton.interactable = true;
				}
				else {
					DownloadButton.interactable = false;
				}
				VersionText.text = DevelopmentStage + " " + GameVersion;
				CodeNameText.text = CodeName;
				/*GUI.Label(rect, DevelopmentStage + " " + GameVersion);
				GUI.Label(new Rect(rect.x - 65, rect.y + 20, rect.width, rect.height), CodeName);*/
			}
		}
		public void OpenDownload() {
			Application.OpenURL("http://zaegames-drabweb.rhcloud.com/sots/");
		}
	}
}