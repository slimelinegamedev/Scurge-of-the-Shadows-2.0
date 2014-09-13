using UnityEngine;
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

public class cInputSetup : MonoBehaviour {
	void Awake() {
		//Buttons
			//Moving
			cInput.SetKey("Move Forward", "W");
			cInput.SetKey("Move Backwards", "S");
			cInput.SetKey("Move Left", "A");
			cInput.SetKey("Move Right", "D");
			//Looking
			cInput.SetKey("Look Up", "Mouse Up");
			cInput.SetKey("Look Down", "Mouse Down");
			cInput.SetKey("Look Left", "Mouse Left");
			cInput.SetKey("Look Right", "Mouse Right");
		//Gameplay
		cInput.SetKey("Attack/Fire 1", "Mouse0");
		cInput.SetKey("Attack/Fire 2", "Mouse1");
		cInput.SetKey("Inventory", "E");
		cInput.SetKey("Interact", "Q");
		cInput.SetKey("Jump", "Space");
		cInput.SetKey("Run", "LeftShift");
		cInput.SetKey("Pause", "Escape");
		//Axis'
		cInput.SetAxis("Horizontal", "Move Left", "Move Right");
		cInput.SetAxis("Vertical", "Move Backwards", "Move Forward");
		cInput.SetAxis("Look X", "Look Left", "Look Right");
		cInput.SetAxis("Look Y", "Look Down", "Look Up");
	}
}