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
using TeamUtility.IO;
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scurge.Util {
	public class Pause : MonoBehaviour {

		public Disable Disable;
		public Objects Objects;
		public Inventory Inventory;

		public GUISkin Skin;
		public Animator PauseAnimator;
		public bool Open = false;
		public bool ShowControlsOptions = false;
		public bool ShowViewOptions = false;
		public bool ShowOptions = false;

		public Slider MouseSensitivityXSlider;
		public Slider MouseSensitivityYSlider;
		public Slider MouseGravityXSlider;
		public Slider MouseGravityYSlider;
		public Slider MouseDeadzoneXSlider;
		public Slider MouseDeadzoneYSlider;

		public InputField MouseSensitivityXDisplay;
		public InputField MouseSensitivityYDisplay;
		public InputField MouseGravityXDisplay;
		public InputField MouseGravityYDisplay;
		public InputField MouseDeadzoneXDisplay;
		public InputField MouseDeadzoneYDisplay;

		public Toggle MouseInvertXToggle;
		public Toggle MouseInvertYToggle;

		//All the controls buttons labels
		public Text MoveForwardControlLabel;
		public Text MoveBackwardsControlLabel;
		public Text MoveLeftControlLabel;
		public Text MoveRightControlLabel;
		public Text LookUpControlLabel;
		public Text LookDownControlLabel;
		public Text LookLeftControlLabel;
		public Text LookRightControlLabel;
		public Text InventoryControlLabel;
		public Text InteractControlLabel;
		public Text JumpControlLabel;
		public Text RunControlLabel;
		public Text PauseControlLabel;

		public bool Calibrating = false;

		public float MouseSensitivityY;
		public float MouseSensitivityX;

		public float MouseGravityY;
		public float MouseGravityX;

		public float MouseDeadzoneY;
		public float MouseDeadzoneX;

		public bool InvertMouseX = false;
		public bool InvertMouseY = false;

		public float MovingSensitivityY;
		public float MovingSensitivityX;
		
		public float MovingGravityY;
		public float MovingGravityX;
		
		public float MovingDeadzoneY;
		public float MovingDeadzoneX;
		
		public bool InvertMovingX = false;
		public bool InvertMovingY = false;

		void Start() {
			Objects.PauseObject.SetActive(false);

			//Mouse
			MouseSensitivityX = cInput.GetAxisSensitivity("Look X");
			MouseSensitivityY = cInput.GetAxisSensitivity("Look Y");

			MouseGravityX = cInput.GetAxisGravity("Look X");
			MouseGravityY = cInput.GetAxisGravity("Look Y");

			MouseDeadzoneX = cInput.GetAxisDeadzone("Look X");
			MouseDeadzoneY = cInput.GetAxisDeadzone("Look Y");

			InvertMouseX = cInput.AxisInverted("Look X");
			InvertMouseY = cInput.AxisInverted("Look Y");

			//Moving
			MovingSensitivityX = cInput.GetAxisSensitivity("Horizontal");
			MovingSensitivityY = cInput.GetAxisSensitivity("Vertical");
			
			MovingGravityX = cInput.GetAxisGravity("Horizontal");
			MovingGravityY = cInput.GetAxisGravity("Vertical");
			
			MovingDeadzoneX = cInput.GetAxisDeadzone("Horizontal");
			MovingDeadzoneY = cInput.GetAxisDeadzone("Vertical");
			
			InvertMovingX = cInput.AxisInverted("Horizontal");
			InvertMouseY = cInput.AxisInverted("Vertical");

			//Initialize sliders
			MouseSensitivityXSlider.value = MouseSensitivityX;
			MouseSensitivityYSlider.value = MouseSensitivityY;
			MouseGravityXSlider.value = MouseGravityX;
			MouseGravityXSlider.value = MouseGravityY;
			MouseDeadzoneXSlider.value = MouseDeadzoneX;
			MouseDeadzoneYSlider.value = MouseDeadzoneY;

			//Init displays
			MouseSensitivityXDisplay.text = MouseSensitivityX.ToString();
			MouseSensitivityYDisplay.text = MouseSensitivityY.ToString();
			MouseGravityXDisplay.text = MouseGravityX.ToString();
			MouseGravityXDisplay.text = MouseGravityY.ToString();
			MouseDeadzoneXDisplay.text = MouseDeadzoneX.ToString();
			MouseDeadzoneYDisplay.text = MouseDeadzoneY.ToString();

			//Init toggles
			MouseInvertXToggle.isOn = cInput.AxisInverted("Horizontal");
			MouseInvertYToggle.isOn = cInput.AxisInverted("Vertical");
		}
		void Update() {
			if(cInput.GetKeyDown("Pause")) {
				PauseUnpauseGame();
			}
			cInput.SetAxisSensitivity("Look X", MouseSensitivityXSlider.value);
			cInput.SetAxisSensitivity("Look Y", MouseSensitivityYSlider.value);

			cInput.SetAxisGravity("Look X", MouseGravityXSlider.value);
			cInput.SetAxisGravity("Look Y", MouseGravityYSlider.value);

			cInput.SetAxisDeadzone("Look X", MouseDeadzoneXSlider.value);
			cInput.SetAxisDeadzone("Look Y", MouseDeadzoneYSlider.value);

			cInput.AxisInverted("Look X", MouseInvertXToggle.isOn);
			cInput.AxisInverted("Look Y", MouseInvertYToggle.isOn);

			cInput.SetAxisSensitivity("Horizontal", MovingSensitivityX);
			cInput.SetAxisSensitivity("Vertical", MovingSensitivityY);
			
			cInput.SetAxisGravity("Horizontal", MovingGravityX);
			cInput.SetAxisGravity("Vertical", MovingGravityY);
			
			cInput.SetAxisDeadzone("Horizontal", MovingDeadzoneX);
			cInput.SetAxisDeadzone("Vertical", MovingDeadzoneY);
			
			cInput.AxisInverted("Horizontal", InvertMovingX);
			cInput.AxisInverted("Vertical", InvertMovingY);

			//Set controls buttons text
			MoveForwardControlLabel.text = cInput.GetText("Move Forward", 1);
			MoveBackwardsControlLabel.text = cInput.GetText("Move Backwards", 1);
			MoveLeftControlLabel.text = cInput.GetText("Move Left", 1);
			MoveRightControlLabel.text = cInput.GetText("Move Right", 1);
			LookUpControlLabel.text = cInput.GetText("Look Up", 1);
			LookDownControlLabel.text = cInput.GetText("Look Down", 1);
			LookLeftControlLabel.text = cInput.GetText("Look Left", 1);
			LookRightControlLabel.text = cInput.GetText("Look Right", 1);
			InventoryControlLabel.text = cInput.GetText("Inventory", 1);
			InteractControlLabel.text = cInput.GetText("Interact", 1);
			JumpControlLabel.text = cInput.GetText("Jump", 1);
			RunControlLabel.text = cInput.GetText("Run", 1);
			PauseControlLabel.text = cInput.GetText("Pause", 1);
		}
		public void PauseUnpauseGame() {
			if(!ShowControlsOptions && !ShowOptions && !Inventory.InventoryOpen) {
				Open = !Open;
				ShowControlsOptions = false;
				ShowOptions = false;
				if(Open) {
					Objects.PauseObject.SetActive(true);
					PauseAnimator.SetTrigger("Pause");
					Disable.DisableObj(true, false);
					Cursor.visible = true;
					Time.timeScale = 0;
				}
				else if(!Open) {
					PauseAnimator.SetTrigger("Pause");
					Disable.EnableObj(true, false);
					Time.timeScale = 1;
					Cursor.visible = false;
					Objects.PauseObject.SetActive(false);
				}
			}
		}
		/*void OnGUI() {
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
		}*/
		public void OpenOptions() {
			ShowOptions = true;
			Objects.PauseMenu.gameObject.SetActive(false);
			Objects.UIOptions.SetActive(true);
		}
		public void ExitOptions() {
			ShowOptions = false;
			Objects.UIOptions.SetActive(false);
			Objects.PauseMenu.gameObject.SetActive(true);
		}
		public void ShowControls() {
			ShowControlsOptions = true;
			Objects.PauseMenu.gameObject.SetActive(false);
			Objects.UIControls.SetActive(true);
		}
		public void HideControls() {
			ShowControlsOptions = false;Objects.PauseMenu.gameObject.SetActive(true);
			Objects.UIControls.SetActive(false);
		}
		public void Quit() {
			#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
		public void ShowCalibration() {
			Objects.CalibrationWindow.SetActive(true);
			Objects.UIControls.SetActive(false);
		}
		public void HideCalibration() {
			Objects.CalibrationWindow.SetActive(false);
			Objects.UIControls.SetActive(true);
		}
		public void CalibrateControls() {
			ShowCalibration();
		}
		public void CallCalibrateFunction() {
			cInput.Calibrate();
			HideCalibration();
		}
		public void RestoreDefaultControls() {
			cInput.ResetInputs();
			Objects.CalibrationWindow.SetActive(false);
			Objects.UIControls.SetActive(true);
		}
		//Function to call to set displays value
		//name: Shortened version of the displays name, eg Mouse Sensitivity X = MSX etc.
		public void SetDisplayValue(string name) {
			//Check names, then apply display values to slider values
			if(name == "MSX") {
				if(MouseSensitivityXSlider.value.ToString().Length > 2) {
					MouseSensitivityXDisplay.text = MouseSensitivityXSlider.value.ToString().Remove(MouseSensitivityXSlider.value.ToString().Length - 2);
				}
				else {
					MouseSensitivityXDisplay.text = MouseSensitivityXSlider.value.ToString();
				}
			}
			else if(name == "MSY") {
				if(MouseSensitivityYSlider.value.ToString().Length > 2) {
					MouseSensitivityYDisplay.text = MouseSensitivityYSlider.value.ToString().Remove(MouseSensitivityYSlider.value.ToString().Length - 2);
				}
				else {
					MouseSensitivityYDisplay.text = MouseSensitivityYSlider.value.ToString();
				}
			}
			else if(name == "MGX") {
				if(MouseGravityXSlider.value.ToString().Length > 2) {
					MouseGravityXDisplay.text = MouseGravityXSlider.value.ToString().Remove(MouseGravityXSlider.value.ToString().Length - 2);
				}
				else {
					MouseGravityXDisplay.text = MouseGravityXSlider.value.ToString();
				}
			}
			else if(name == "MGY") {
				if(MouseGravityYSlider.value.ToString().Length > 2) {
					MouseGravityYDisplay.text = MouseGravityYSlider.value.ToString().Remove(MouseGravityYSlider.value.ToString().Length - 2);
				}
				else {
					MouseGravityYDisplay.text = MouseGravityYSlider.value.ToString();
				}
			}
			else if(name == "MDX") {
				if(MouseDeadzoneXSlider.value.ToString().Length > 2) {
					MouseDeadzoneXDisplay.text = MouseDeadzoneXSlider.value.ToString().Remove(MouseDeadzoneXSlider.value.ToString().Length - 2);
				}
				else {
					MouseDeadzoneXDisplay.text = MouseDeadzoneXSlider.value.ToString();
				}
			}
			else if(name == "MDY") {
				if(MouseDeadzoneYSlider.value.ToString().Length > 2) {
					MouseDeadzoneYDisplay.text = MouseDeadzoneYSlider.value.ToString().Remove(MouseDeadzoneYSlider.value.ToString().Length - 2);
				}
				else {
					MouseDeadzoneYDisplay.text = MouseDeadzoneYSlider.value.ToString();
				}
			}
		}
		public void SetSliderValue(string name) {
			if(name == "MSX") {
				if(float.Parse(MouseSensitivityXDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseSensitivityXDisplay.text = "20";
				}
				MouseSensitivityXSlider.value = float.Parse(MouseSensitivityXDisplay.text, CultureInfo.InvariantCulture);
			}
			else if(name == "MSY") {
				if(float.Parse(MouseSensitivityYDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseSensitivityYDisplay.text = "20";
				}
				MouseSensitivityYSlider.value = float.Parse(MouseSensitivityYDisplay.text, CultureInfo.InvariantCulture);
			}
			else if(name == "MGX") {
				if(float.Parse(MouseGravityXDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseGravityXDisplay.text = "20";
				}
				MouseGravityXSlider.value = float.Parse(MouseGravityXDisplay.text, CultureInfo.InvariantCulture);
			}
			else if(name == "MGY") {
				if(float.Parse(MouseGravityYDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseGravityYDisplay.text = "20";
				}
				MouseGravityYSlider.value = float.Parse(MouseGravityYDisplay.text, CultureInfo.InvariantCulture);
			}
			else if(name == "MDX") {
				if(float.Parse(MouseDeadzoneXDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseDeadzoneXDisplay.text = "20";
				}
				MouseDeadzoneXSlider.value = float.Parse(MouseDeadzoneXDisplay.text, CultureInfo.InvariantCulture);
			}
			else if(name == "MDY") {
				if(float.Parse(MouseDeadzoneYDisplay.text, CultureInfo.InvariantCulture) > 20) {
					MouseDeadzoneYDisplay.text = "20";
				}
				MouseDeadzoneYSlider.value = float.Parse(MouseDeadzoneYDisplay.text, CultureInfo.InvariantCulture);
			}
		}
		public void SetControl(string name) {
			cInput.ChangeKey(name, 1);
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