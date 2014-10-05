using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

using GUILayout = transfluent.guiwrapper.GUILayout;
using GUI = transfluent.guiwrapper.GUI;
using transfluent;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scurge.UI {
	public class Menu : MonoBehaviour {
		public GUISkin Skin;
		public Wander Wander;

		public bool visible = true;
		public bool savesVisible = false;
		public bool optionsVisible = false;
		public bool languageVisible = false;

		public float playX = 10;
		public float quitX = 10;
		public float languageX = 10;

		public string saveOne;
		public string saveTwo;
		public string saveThree;

		void Update() {
			if(visible) {
				if(!savesVisible && !optionsVisible) {
					if(new Rect(10, 540, 200, 50).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						print("Inside Play Button!");
						playX = 30;
					}
					else {
						playX = 10;
					}
					if(new Rect(10, 600, 200, 50).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						print("Inside Language Button!");
						languageX = 30;
					}
					else {
						languageX = 10;
					}
					if(new Rect(10, 660, 200, 50).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						print("Inside Quit Button!");
						quitX = 30;
					}
					else {
						quitX = 10;
					}
				}
				if(savesVisible) {

				}
				if(optionsVisible) {
					
				}
			}
		}

		void OnGUI() {
			GUI.skin = Skin;
			if(visible) {
				GUI.Label(new Rect(640, 10, 10, 200), "<size=60>Scurge of the Shadows 2.0</size>", "Center Label");
				if(!savesVisible && !optionsVisible && !languageVisible) {
					if(GUI.Button(new Rect(playX, 540, 200, 50), "<size=40>Play</size>", "Menu Button")) {
						ShowSaves(true);
					}
					if(GUI.Button(new Rect(languageX, 600, 200, 50), "<size=40>Language</size>", "Menu Button")) {
						ShowLanguage();
					}
					if(GUI.Button(new Rect(quitX, 660, 200, 50), "<size=40>Exit</size>", "Menu Button")) {
						#if UNITY_EDITOR
						EditorApplication.isPlaying = false;
						#else
						Application.Quit();
						#endif
					}
				}
				if(languageVisible) {
					if(GUI.Button(new Rect(10, 660, 200, 50), "<size=40>Cancel</size>", "Menu Button")) {
						ShowLanguage();
					}
				}
				if(savesVisible) {
					if(GUI.Button(new Rect(562.5f, 300, 155, 50), "<size=40>" + saveOne + "</size>", "Upper Left Button")) {

					}
					if(GUI.Button(new Rect(562.5f, 360, 155, 50), "<size=40>" + saveTwo + "</size>", "Upper Left Button")) {

					}
					if(GUI.Button(new Rect(562.5f, 420, 155, 50), "<size=40>" + saveThree + "</size>", "Upper Left Button")) {

					}
				}
				if(optionsVisible) {
					
				}
			}
		}
		public void ShowLanguage() {
			languageVisible = !languageVisible;
			print("Selecting Language");
		}
		public void ShowSaves(bool autoPlay) {
			if(!autoPlay) {
				savesVisible = true;
			}
			else {
				Application.LoadLevel(1);
			}
		}
		public void ShowOptions() {
			//optionsVisible = true;
		}
	}
}