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

		public string[] ViewOptionsGridText;
		//Mouse, Keyboard, Joystick
		public int SelectedViewOptions;

		public string[] MoveOptionsGridText;
		//Keyboard, Joystick
		public int SelectedMoveOptions;

		public bool Mouse = true;
		public bool Keyboard = false;
		public bool Joystick = false;
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
			if(Open) {
				if(SelectedViewOptions == 0) {
					Mouse = true;
					Keyboard = false;
					Joystick = false;
				}
				else if(SelectedViewOptions == 1) {
					Mouse = false;
					Keyboard = true;
					Joystick = false;
				}
				else if(SelectedViewOptions == 2) {
					Mouse = false;
					Keyboard = false;
					Joystick = true;
				}

				if(SelectedMoveOptions == 0) {
					InputManager.GetAxisConfiguration("Default", "Horizontal").type = InputType.DigitalAxis;
					InputManager.GetAxisConfiguration("Default", "Vertical").type = InputType.DigitalAxis;
				}
				else if(SelectedMoveOptions == 1) {
					InputManager.GetAxisConfiguration("Default", "Horizontal").type = InputType.AnalogAxis;
					InputManager.GetAxisConfiguration("Default", "Vertical").type = InputType.AnalogAxis;
				}
			}
			if(Mouse) {
				InputManager.GetAxisConfiguration("Default", "Mouse X").type = InputType.MouseAxis;
				InputManager.GetAxisConfiguration("Default", "Mouse Y").type = InputType.MouseAxis;
			}
			else if(Keyboard) {
				InputManager.GetAxisConfiguration("Default", "Mouse X").type = InputType.DigitalAxis;
				InputManager.GetAxisConfiguration("Default", "Mouse Y").type = InputType.DigitalAxis;
			}
			else if(Joystick) {
				InputManager.GetAxisConfiguration("Default", "Mouse X").type = InputType.AnalogAxis;
				InputManager.GetAxisConfiguration("Default", "Mouse Y").type = InputType.AnalogAxis;
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
				GUI.Label(new Rect(425, 250, 400, 50), "Look With");
				SelectedViewOptions = GUI.SelectionGrid(new Rect(425, 280, 400, 50), SelectedViewOptions, ViewOptionsGridText, 3);
				InputManager.GetAxisConfiguration("Default", "Mouse X").invert  = GUI.Toggle(new Rect(425, 340, 400, 30), InputManager.GetAxisConfiguration("Default", "Mouse X").invert, "Invert Looking X");
				InputManager.GetAxisConfiguration("Default", "Mouse Y").invert  = GUI.Toggle(new Rect(425, 370, 400, 30), InputManager.GetAxisConfiguration("Default", "Mouse Y").invert, "Invert Looking Y");

				GUI.Label(new Rect(425, 380 + 20, 400, 50), "Move With");
				SelectedMoveOptions = GUI.SelectionGrid(new Rect(425, 410 + 20, 400, 50), SelectedMoveOptions, MoveOptionsGridText, 2);
				InputManager.GetAxisConfiguration("Default", "Horizontal").invert  = GUI.Toggle(new Rect(425, 470 + 20, 400, 30), InputManager.GetAxisConfiguration("Default", "Horizontal").invert, "Invert Movement X");
				InputManager.GetAxisConfiguration("Default", "Vertical").invert  = GUI.Toggle(new Rect(425, 500 + 20, 400, 30), InputManager.GetAxisConfiguration("Default", "Vertical").invert, "Invert Movement Y");

				if(GUI.Button(new Rect(425, 570, 217.5f, 50), "Save Config")) {
					InputManager.Save();
				}
				if(GUI.Button(new Rect(637.5f, 570, 217.5f, 50), "Load Config")) {
					InputManager.Load();
				}
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
					if(GUI.Button(new Rect(640, 150, 215, 50), "Up")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Vertical", true);
						ScanIndexControls = 0;
						//SetKey(InputManager.GetAxisConfiguration("Default", "Vertical"), CurrentKeyScanned, true);
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 1;
					if(GUI.Button(new Rect(640, 210, 215, 50), "Down")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Vertical", false);
						ScanIndexControls = 1;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 2;
					if(GUI.Button(new Rect(640, 270, 215, 50), "Left")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Horizontal", false);
						ScanIndexControls = 2;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 3;
					if(GUI.Button(new Rect(640, 330, 215, 50), "Right")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Horizontal", true);
						ScanIndexControls = 3;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 4;
					if(GUI.Button(new Rect(640, 390, 215, 50), "Look Up")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse Y", true);
						ScanIndexControls = 4;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 5;
					if(GUI.Button(new Rect(640, 450, 215, 50), "Look Down")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse Y", false);
						ScanIndexControls = 5;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 6;
					if(GUI.Button(new Rect(640, 510, 215, 50), "Look Left")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse X", false);
						ScanIndexControls = 6;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexControls != 7;
					if(GUI.Button(new Rect(640, 570, 215, 50), "Look Right")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Mouse X", true);
						ScanIndexControls = 7;
					}
					GUI.enabled = true;

					//Labels to show Buttons
					GUI.Label(new Rect(425, 160, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Vertical").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 210 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Vertical").negative.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 270 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Horizontal").negative.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 330 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Horizontal").positive.ToString() + "</size>", "Center Label");
					if(Mouse) {
						GUI.Label(new Rect(425, 390 + 10, 215, 50), "<size=15>Mouse Up</size>", "Center Label");
						GUI.Label(new Rect(425, 450 + 10, 215, 50), "<size=15>Mouse Down</size>", "Center Label");
						GUI.Label(new Rect(425, 510 + 10, 215, 50), "<size=15>Mouse Left</size>", "Center Label");
						GUI.Label(new Rect(425, 570 + 10, 215, 50), "<size=15>Mouse Right</size>", "Center Label");
					}
					else if(Keyboard) {
						GUI.Label(new Rect(425, 390 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse Y").positive.ToString() + "</size>", "Center Label");
						GUI.Label(new Rect(425, 450 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse Y").negative.ToString() + "</size>", "Center Label");
						GUI.Label(new Rect(425, 510 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse X").negative.ToString() + "</size>", "Center Label");
						GUI.Label(new Rect(425, 570 + 10, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse X").positive.ToString() + "</size>", "Center Label");
					}
					else if(Joystick) {
						GUI.Label(new Rect(425, 390 + 10, 215, 50), "<size=15>Joystick1Up</size>", "Center Label");
						GUI.Label(new Rect(425, 450 + 10, 215, 50), "<size=15>Joystick1Down</size>", "Center Label");
						GUI.Label(new Rect(425, 510 + 10, 215, 50), "<size=15>Joystick1Left</size>", "Center Label");
						GUI.Label(new Rect(425, 570 + 10, 215, 50), "<size=15>Joystick1Right</size>", "Center Label");
					}
					/*GUI.Label(new Rect(425, 390, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse Y").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 450, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse Y").negative.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 510, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse X").negative.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 570, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Mouse X").positive.ToString() + "</size>", "Center Label");*/
				}
				else if(!ShowViewOptions) {
					if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
						ShowControlsOptions = false;
					}
					if(GUI.Button(new Rect(425, 150, 430, 50), "View/Movement Controls")) {
						ShowViewOptions = true;
					}
					GUI.enabled = ScanIndexButtons != 0;
					if(GUI.Button(new Rect(640, 210, 215, 50), "Attack/Fire 1")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Left", true);
						ScanIndexButtons = 0;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 1;
					if(GUI.Button(new Rect(640, 270, 215, 50), "Attack/Fire 2")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Right", true);
						ScanIndexButtons = 1;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 2;
					if(GUI.Button(new Rect(640, 330, 215, 50), "Inventory")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Inventory", true);
						ScanIndexButtons = 2;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 3;
					if(GUI.Button(new Rect(640, 390, 215, 50), "Interact")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Interact", true);
						ScanIndexButtons = 3;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 4;
					if(GUI.Button(new Rect(640, 450, 215, 50), "Jump")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Jump", true);
						ScanIndexButtons = 4;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 5;
					if(GUI.Button(new Rect(640, 510, 215, 50), "Run")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Run", true);
						ScanIndexButtons = 5;
					}
					GUI.enabled = true;
					GUI.enabled = ScanIndexButtons != 6;
					if(GUI.Button(new Rect(640, 570, 215, 50), "Pause")) {
						InputManager.StartKeyScan(HandleKeyScanResult, 10.0f, null, "Pause", true);
						ScanIndexButtons = 6;
					}
					GUI.enabled = true;

					//Labels to show Buttons
					GUI.Label(new Rect(425, 220, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Left").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 280, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Right").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 340, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Inventory").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 400, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Interact").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 460, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Jump").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 520, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Run").positive.ToString() + "</size>", "Center Label");
					GUI.Label(new Rect(425, 580, 215, 50), "<size=15>" + InputManager.GetAxisConfiguration("Default", "Pause").positive.ToString() + "</size>", "Center Label");
				}
			}
		}
	}
}