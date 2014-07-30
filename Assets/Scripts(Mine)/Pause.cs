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
		public int ScanIndexControls;
		public int ScanIndexButtons;
		public bool Open = false;

		public bool Mouse = true;
		public bool ShowControlsOptions = false;
		public bool ShowViewOptions = false;
		public bool ShowOptions = false;

		public bool IsValidKey(KeyCode key) {
			if(key == KeyCode.LeftApple || key == KeyCode.RightApple) {
				return false;
			}
			if(key == KeyCode.LeftWindows || key == KeyCode.RightWindows) {
				return false;
			}
			return true;
		}
		public void SetKey(AxisConfiguration button, KeyCode key, bool positive) {
			if(positive) {
				button.positive = key;
			}
			else if(!positive) {
				button.negative = key;
			}
		}
		private bool HandleKeyScanResult(KeyCode key, params object[] args) {
			if(!IsValidKey(key))
				return false;

			string axisName = (string)args[0];
			bool positive = (bool)args[1];

			if(key != KeyCode.None) {
				AxisConfiguration axisConfig = InputManager.GetAxisConfiguration("Default", axisName);
				if(axisConfig != null) {
					SetKey(axisConfig, key, positive);
				}
			}	
			ScanIndexControls = -1;
			ScanIndexButtons = -1;
			return true;
		}

		void Update() {
			if(Mouse) {
				InputManager.GetAxisConfiguration("Default", "Mouse X").type = InputType.MouseAxis;
				InputManager.GetAxisConfiguration("Default", "Mouse Y").type = InputType.MouseAxis;
			}
			else if(!Mouse) {
				InputManager.GetAxisConfiguration("Default", "Mouse X").type = InputType.DigitalAxis;
				InputManager.GetAxisConfiguration("Default", "Mouse Y").type = InputType.DigitalAxis;
			}
			if(InputManager.GetButtonDown("Pause")) {
				Open = !Open;
				if(ShowControlsOptions) {
					ShowControlsOptions = false;
				}
				if(ShowOptions) {
					ShowOptions = false;
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
			if(Open && !ShowControlsOptions && !ShowOptions) {
				GUI.Box(new Rect(420, 297.5f, 440, 125), "Paused");
				if(GUI.Button(new Rect(500- 70, 327.5f, 140, 85), "Resume")) {
					Open = !Open;
					Disable.EnableObj(true);
					Time.timeScale = 1;
					Screen.showCursor = false;
				}
				if(GUI.Button(new Rect(640 - 70, 327.5f, 140, 85), "Options")) {
					ShowOptions = true;
				}
				if(GUI.Button(new Rect(780 - 70, 327.5f, 140, 85), "Control Setup")) {
					ShowControlsOptions = true;
				}
			}
			if(Open && ShowOptions) {
				GUI.Box(new Rect(415, 60, 450, 570), "Options");
				if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
					ShowOptions = false;
				}
				GUI.Label(new Rect(425, 160, 215, 50), "Mouse Sensitivity X");
				GUI.Label(new Rect(425, 220, 215, 50), "Mouse Sensitivity Y");
				InputManager.GetAxisConfiguration("Default", "Mouse X").sensitivity = GUI.HorizontalSlider(new Rect(640, 160, 215, 50), InputManager.GetAxisConfiguration("Default", "Mouse X").sensitivity, 0.0F, 20.0F);
				InputManager.GetAxisConfiguration("Default", "Mouse Y").sensitivity = GUI.HorizontalSlider(new Rect(640, 220, 215, 50), InputManager.GetAxisConfiguration("Default", "Mouse Y").sensitivity, 0.0F, 20.0F);
				Mouse = GUI.Toggle(new Rect(425, 280, 215, 50), Mouse, "Using Mouse");
			}
			if(Open && ShowControlsOptions) {
				//11 Controls
				GUI.Box(new Rect(415, 60, 450, 570), "Control Setup");
				if(ShowViewOptions) {
					if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
						ShowViewOptions = false;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 0;
					if(GUI.Button(new Rect(425, 150, 215, 50), "Up")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Vertical", true);
						ScanIndexControls = 0;
						//SetKey(InputManager.GetAxisConfiguration("Default", "Vertical"), CurrentKeyScanned, true);
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 1;
					if(GUI.Button(new Rect(425, 210, 215, 50), "Down")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Vertical", false);
						ScanIndexControls = 1;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 2;
					if(GUI.Button(new Rect(425, 270, 215, 50), "Left")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Horizontal", false);
						ScanIndexControls = 2;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 3;
					if(GUI.Button(new Rect(425, 330, 215, 50), "Right")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Horizontal", true);
						ScanIndexControls = 3;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 4;
					if(GUI.Button(new Rect(425, 390, 215, 50), "Look Up")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse Y", true);
						ScanIndexControls = 4;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 5;
					if(GUI.Button(new Rect(425, 450, 215, 50), "Look Down")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse Y", false);
						ScanIndexControls = 5;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 6;
					if(GUI.Button(new Rect(425, 510, 215, 50), "Look Left")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse X", false);
						ScanIndexControls = 6;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 7;
					if(GUI.Button(new Rect(425, 570, 215, 50), "Look Right")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse X", true);
						ScanIndexControls = 7;
					}
					GUI.enabled = true;
				}
				else if(!ShowViewOptions) {
					if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
						ShowControlsOptions = false;
					}
					if(GUI.Button(new Rect(425, 150, 430, 50), "View/Movement Controls")) {
						ShowViewOptions = true;
					}
					GUI.enabled = ScanIndexButtons != 0;
					if(GUI.Button(new Rect(425, 210, 215, 50), "Attack/Fire 1")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Left", true);
						ScanIndexButtons = 0;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 1;
					if(GUI.Button(new Rect(425, 270, 215, 50), "Attack/Fire 2")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Right", true);
						ScanIndexButtons = 1;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 2;
					if(GUI.Button(new Rect(425, 330, 215, 50), "Inventory")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Inventory", true);
						ScanIndexButtons = 2;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 3;
					if(GUI.Button(new Rect(425, 390, 215, 50), "Interact")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Interact", true);
						ScanIndexButtons = 3;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 4;
					if(GUI.Button(new Rect(425, 450, 215, 50), "Jump")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Jump", true);
						ScanIndexButtons = 4;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 5;
					if(GUI.Button(new Rect(425, 510, 215, 50), "Run")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Run", true);
						ScanIndexButtons = 5;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 6;
					if(GUI.Button(new Rect(425, 570, 215, 50), "Pause")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Pause", true);
						ScanIndexButtons = 6;
					}
					GUI.enabled = true;
				}
			}
		}
	}
}