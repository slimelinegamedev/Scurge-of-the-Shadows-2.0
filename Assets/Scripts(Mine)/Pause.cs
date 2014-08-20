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
		public bool ShowViewOptions = false;
		public bool ShowOptions = false;

		public bool Calibrating = false;

		public float MouseSensitivityY;
		public float MouseSensitivityX;

		public float MouseGravityY;
		public float MouseGravityX;

		public float MouseDeadzoneY;
		public float MouseDeadzoneX;

		public bool InvertMouseX = false;
		public bool InvertMouseY = false;

		void Start() {
			MouseSensitivityX = cInput.GetAxisSensitivity("Look X");
			MouseSensitivityY = cInput.GetAxisSensitivity("Look Y");

			MouseGravityX = cInput.GetAxisGravity("Look X");
			MouseGravityY = cInput.GetAxisGravity("Look Y");

			MouseDeadzoneX = cInput.GetAxisDeadzone("Look X");
			MouseDeadzoneY = cInput.GetAxisDeadzone("Look Y");

			InvertMouseX = cInput.AxisInverted("Look X");
			InvertMouseY = cInput.AxisInverted("Look Y");
		}
		void Update() {
			if(cInput.GetKeyDown("Pause")) {
				Open = !Open;
				if(ShowControlsOptions) {
					ShowControlsOptions = false;
				}
				if(ShowOptions) {
					ShowOptions = false;
				}
				if(Open) {
					Disable.DisableObj(true, false);
					Time.timeScale = 0;
					Screen.showCursor = true;
				}
				else if(!Open) {
					Disable.EnableObj(true, false);
					Time.timeScale = 1;
					Screen.showCursor = false;
				}
			}
			cInput.SetAxisSensitivity("Look X", MouseSensitivityX);
			cInput.SetAxisSensitivity("Look Y", MouseSensitivityY);

			cInput.SetAxisGravity("Look X", MouseGravityX);
			cInput.SetAxisGravity("Look Y", MouseGravityY);

			cInput.SetAxisDeadzone("Look X", MouseDeadzoneX);
			cInput.SetAxisDeadzone("Look Y", MouseDeadzoneY);

			cInput.AxisInverted("Look X", InvertMouseX);
			cInput.AxisInverted("Look Y", InvertMouseY);
		}
		void OnGUI() {
			GUI.skin = Skin;
			if(!Calibrating) {
				GUI.enabled = true;
			}
			else if(Calibrating) {
				GUI.enabled = false;
			}
			if(Open && ShowOptions) {
				GUI.Box(new Rect(415, 60, 450, 570), "Options", "Box");
				if(GUI.Button(new Rect(425, 90, 430, 50), "Back")) {
					ShowOptions = false;
				}
				GUILayout.BeginArea(new Rect(425, 170, 430, 570));
					GUILayout.BeginVertical();
						//Sensitivity
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Sensitivity Y", GUILayout.Width(230));
						MouseSensitivityY = GUILayout.HorizontalSlider(MouseSensitivityY, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Sensitivity X", GUILayout.Width(230));
						MouseSensitivityX = GUILayout.HorizontalSlider(MouseSensitivityX, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						//Gravity
						GUILayout.Space(20);
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Gravity Y", GUILayout.Width(230));
						MouseGravityY = GUILayout.HorizontalSlider(MouseGravityY, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Gravity X", GUILayout.Width(230));
						MouseGravityX = GUILayout.HorizontalSlider(MouseGravityX, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						//Deadzone
						GUILayout.Space(20);
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Deadzone Y", GUILayout.Width(230));
						MouseDeadzoneY = GUILayout.HorizontalSlider(MouseDeadzoneY, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Label("Looking Deadzone X", GUILayout.Width(230));
						MouseDeadzoneX = GUILayout.HorizontalSlider(MouseDeadzoneX, 0f, 10f, GUILayout.Width(190));
						GUILayout.EndHorizontal();
						GUILayout.Space(20);
						InvertMouseX = GUILayout.Toggle(InvertMouseX, "Invert Looking X", GUILayout.Width(190));
						GUILayout.Space(10);
						InvertMouseY = GUILayout.Toggle(InvertMouseY, "Invert Looking Y", GUILayout.Width(190));
					GUILayout.EndVertical();
				GUILayout.EndArea();
			}
			if(Open && !ShowControlsOptions && !ShowOptions) {
				GUI.Box(new Rect(420, 297.5f, 440, 125), "Paused");
				if(GUI.Button(new Rect(500- 70, 327.5f, 140, 85), "Resume")) {
					Open = !Open;
					Disable.EnableObj(true, false);
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
			if(Open && ShowControlsOptions) {
				GUI.Box(new Rect(40, 40, 1200, 652), "Control Setup", "Box");
				if(GUI.Button(new Rect(50, 632, 200, 50), "Back")) {
					ShowControlsOptions = false;
				}
				if(GUI.Button(new Rect(50, 572, 200, 50), "Defaults")) {
					cInput.ResetInputs();
				}
				if(GUI.Button(new Rect(50, 512, 200, 50), "Calibrate")) {
					Calibrating = true;
				}
				//Buttons For Setup
				GUILayout.BeginArea(new Rect(280, 90, 430, 1280));
					GUILayout.BeginVertical();
					//Movement And Looking
						InputButton("Move Forward", "Move Forward", 1);
						InputButton("Move Backwards", "Move Backwards", 1);
						InputButton("Move Left", "Move Left", 1);
						InputButton("Move Right", "Move Right", 1);
						InputButton("Look Up", "Look Up", 1);
						InputButton("Look Down", "Look Down", 1);
						InputButton("Look Left", "Look Left",  1);
						InputButton("Look Right",  "Look Right", 1);
					//Other
						InputButton("Attack/Fire 1", "Primary Attack", 1);
						InputButton("Attack/Fire 2", "Second Attack", 1);
					GUILayout.EndVertical();
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(760, 90, 430, 1280));
					GUILayout.BeginVertical();
						InputButton("Inventory", "Inventory", 1);
						InputButton("Interact", "Interact", 1);
						InputButton("Jump", "Jump", 1);
						InputButton("Run", "Run", 1);
						InputButton("Pause", "Pause", 1);
					GUILayout.EndVertical();
				GUILayout.EndArea();
				
			}
			GUI.enabled = true;
			if(Open && Calibrating) {
				GUI.Box(new Rect(515, 250, 250, 300), "Calibrate Joysticks\n\n\n\nPut all joysticks in\n\ndefault position\n\nand hit\n\nCalibrate");
				if(GUI.Button(new Rect(525, 490, 230, 50), "Calibrate")) {
					cInput.Calibrate();
					Calibrating = false;
				}
			}
		}
		public void InputButton(string name, string display, int primary) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(display);
			if(GUILayout.Button(cInput.GetText(name, primary), GUILayout.Height(50), GUILayout.Width(250))) {
				cInput.ChangeKey(name, primary);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(6);
		}
	}
}