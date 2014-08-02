using UnityEngine;
using UnityEditor;
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

namespace Scurge.Editor {
	public class MeshCombiner : EditorWindow {
		[MenuItem ("Utility/Mesh Combiner")]
		static void Init () {
			EditorWindow window = EditorWindow.GetWindow(typeof (MeshCombiner));
			window.title = "Mesh Combiner Utility";
		}
	}
}