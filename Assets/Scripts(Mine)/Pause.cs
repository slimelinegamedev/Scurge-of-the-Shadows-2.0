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

namespace Scurge.Util {
	public class Pause : MonoBehaviour {

		public Disable Disable;

		public GUISkin Skin;
		public bool Open = false;

		public bool ShowControlsOptions = false;

		void Update() {
			if(InputManager.GetButtonDown("Pause")) {
				Open = !Open;
				if(ShowControlsOptions) {
					ShowControlsOptions = false;
				}
				if(Open) {
					Disable.DisableObj(true);
					Time.timeScale = 0;
					Screen.showCursor = true;
				}
				else if(!Open) {
					Disable.EnableObj(true);
					Time.timeScale = 1;
					Screen.showCursor = false;
				}
			}
		}
		void OnGUI() {
			GUI.skin = Skin;
			if(Open && !ShowControlsOptions) {
				GUI.Box(new Rect(490, 297.5f, 300, 125), "Paused");
				if(GUI.Button(new Rect(500, 327.5f, 140, 85), "Resume")) {
					Open = !Open;
					Disable.EnableObj(true);
					Time.timeScale = 1;
					Screen.showCursor = false;
				}
				if(GUI.Button(new Rect(640, 327.5f, 140, 85), "Control Setup")) {
					ShowControlsOptions = true;
				}
			}
			if(Open && ShowControlsOptions) {
				GUI.Box(new Rect(415, 60, 450, 600), "Control Setup");
				if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
					ShowControlsOptions = false;
				}
			}
		}
	}
}