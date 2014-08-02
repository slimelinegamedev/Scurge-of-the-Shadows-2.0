using UnityEngine;
using UnityEditor;
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

	public enum AllEnemys {
		Skeleton,
		Snake,
		Bat,
		Sludge,
		Assassin,
		SkeletalWarrior,
		Goblin,
		Wizard,
		Ghoul,
		Amount
	}

	public enum ScriptType {
		Initialization,
		AI,
		HealthVariables,
		EnemyStats,
		Animator
	}

	public class EnemyCreator : EditorWindow {
		[MenuItem ("Window/Enemy Editor %#v")]
		static void Init() {
			EditorWindow window = EditorWindow.GetWindow(typeof (EnemyCreator));
			window.title = "Enemy Editor";
		}

		public AllEnemys allEnemys;
		public ScriptType scriptType;

		public bool DropdownOpen = false;
		public List<GameObject> AllEnemysObjects = new List<GameObject>(1);

		public Scurge.Enemy.AI CurAI;
		public Animator CurAnimator;
		public HealthVariables CurHealthVariables;
		public EnemyStats CurEnemyStats;

		public void OnGUI() {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Enemy Stats Editor");
			scriptType = (ScriptType)EditorGUILayout.EnumPopup("What Script", scriptType);
			allEnemys = (AllEnemys)EditorGUILayout.EnumPopup("Enemys", allEnemys);
			GUILayout.EndHorizontal();
			if(scriptType == ScriptType.Initialization) {
				DropdownOpen = EditorGUILayout.Foldout(DropdownOpen, "Enemy Objects");
				if(DropdownOpen) {
					GUILayout.BeginHorizontal();
						GUILayout.Space(30);
							GUILayout.BeginHorizontal();
								
							GUILayout.EndHorizontal();
						GUILayout.Label(AllEnemysObjects.Count.ToString());
					GUILayout.EndHorizontal();
				}
			}
			if(scriptType == ScriptType.Animator) {
				if(GUILayout.Button("Open Animator")) {
					EditorApplication.ExecuteMenuItem("Window/Layouts/Animating");
				}
				if(GUILayout.Button("Restore Layout")) {
					EditorApplication.ExecuteMenuItem("Window/Layouts/All Purpose Layout");
				}
			}
		}
		public void OnInspectorUpdate() {
			Repaint();
		}
	}
}