using UnityEngine;
using UnityEditor;
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

public class CustomPlay : MonoBehaviour {
	[MenuItem("Tools/Scurge/Play/Play %#LEFTr")]
	public static void Play() {
		if(EditorApplication.SaveCurrentSceneIfUserWantsTo()) {
			EditorApplication.ExecuteMenuItem("Edit/Play");
		}
	}
}
