using UnityEngine;
using UnityEditor;
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

public class OpenScenes : Editor {
	[MenuItem("Tools/Scurge/Open Scene/Main &1")]
	public static void OpenMain() {
		OpenScene("Main");
	}
	[MenuItem("Tools/Scurge/Open Scene/Menu &2")]
	public static void OpenMenu() {
		OpenScene("Menu");
	}
	[MenuItem("Tools/Scurge/Open Scene/Village &3")]
	public static void OpenVillage() {
		OpenScene("Village");
	}
	public static void OpenScene(string sceneName) {
		if(EditorApplication.SaveCurrentSceneIfUserWantsTo()) {
			EditorApplication.OpenScene("Assets/Scenes/" + sceneName + ".unity");
		}
	}
}
