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

public class MouseMover : MonoBehaviour {

	public bool enabled = true;
	public Vector2 mousePosition;

	void Update() {
		if(mousePosition.x > 0 && mousePosition.x < Screen.width) {
			mousePosition.x += cInput.GetAxis("Look X");
		}
		if(mousePosition.y > 0 && mousePosition.y < Screen.height) {
			mousePosition.y += cInput.GetAxis("Look Y");
		}
	}
}