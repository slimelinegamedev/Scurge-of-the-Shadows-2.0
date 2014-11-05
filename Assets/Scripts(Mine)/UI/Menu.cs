using UnityEngine;
using UnityEngine.UI;
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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scurge.UI {
	public class Menu : MonoBehaviour {
		public GUISkin Skin;
		public Wander Wander;

		public List<Button> MainButtons;
		public Button languageCancelButton;
		public Button languageButton;

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

		void Start() {
			languageCancelButton.interactable = false;
		}

		void Update() {
			if(visible) {
				if(savesVisible) {

				}
				if(optionsVisible) {
					
				}
			}
		}

		void OnGUI() {
			GUI.skin = Skin;
			if(visible) {
				//Old GUI
				//GUI.Label(new Rect(640, 10, 10, 200), "<size=60>Scurge of the Shadows 2.0</size>", "Center Label");
				if(!savesVisible && !optionsVisible && !languageVisible) {
					/*if(GUI.Button(new Rect(playX, 540, 200, 50), "<size=40>Play</size>", "Menu Button")) {
						ShowSaves(true);
					}
					if(GUI.Button(new Rect(languageX, 600, 200, 50), "<size=40>Language</size>", "Menu Button")) {
						ShowLanguage();
					}
					if(GUI.Button(new Rect(quitX, 660, 200, 50), "<size=40>Exit</size>", "Menu Button")) {

					}*/
				}
				if(languageVisible) {
					/*if(GUI.Button(new Rect(10, 660, 200, 50), "<size=40>Cancel</size>", "Menu Button")) {
						ShowLanguage();
					}*/
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
		public void Quit() {
			#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
		public void ShowLanguage() {
			languageVisible = true;
			languageCancelButton.interactable = true;
			languageButton.interactable = false;
			foreach(Button button in MainButtons) {
				button.interactable = false;
			}
		}
		public void HideLanguage() {
			languageVisible = false;
			languageCancelButton.interactable = false;
			languageButton.interactable = true;
			foreach(Button button in MainButtons) {
				button.interactable = true;
			}
		}
		public void Play(bool autoPlay) {
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