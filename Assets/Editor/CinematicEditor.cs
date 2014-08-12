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

[CustomEditor(typeof(Cinematic))]
public class CinematicEditor : Editor {

	public bool ShowAssignmentFold = false;
	public bool ShowDebugFold = false;
	public bool ShowDialogue = false;
	public int RemoveIndex = 0;

	public override void OnInspectorGUI() {
		Cinematic Cinematic = (Cinematic)target;
		ShowAssignmentFold = EditorGUILayout.Foldout(ShowAssignmentFold, "Basic Variables");
		if(ShowAssignmentFold) {
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.Disable = (Disable)EditorGUILayout.ObjectField("Disable Script", Cinematic.Disable, typeof(Disable), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.Objects = (Objects)EditorGUILayout.ObjectField("Objects Script", Cinematic.Objects, typeof(Objects), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.CinemaCamera = (Camera)EditorGUILayout.ObjectField("Cinematic Camera", Cinematic.CinemaCamera, typeof(Camera), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.DarkBack = (Camera)EditorGUILayout.ObjectField("Black Background Camera", Cinematic.DarkBack, typeof(Camera), true);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.Skin = (GUISkin)EditorGUILayout.ObjectField("GUI Skin", Cinematic.Skin, typeof(GUISkin), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.BossAI = (Scurge.Enemy.AI)EditorGUILayout.ObjectField("Boss AI Script", Cinematic.BossAI, typeof(Scurge.Enemy.AI), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.BossEnemyStats = (EnemyStats)EditorGUILayout.ObjectField("Boss Enemy Stats Script", Cinematic.BossEnemyStats, typeof(EnemyStats), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.BossAI = (Scurge.Enemy.AI)EditorGUILayout.ObjectField("Boss Look Script", Cinematic.BossAI, typeof(Scurge.Enemy.AI), true);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.BeginWaitTime = EditorGUILayout.FloatField("Time To Wait Before Activating Cutscene", Cinematic.BeginWaitTime);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.CinematicLength = EditorGUILayout.FloatField("The Length Of The Cinematic", Cinematic.CinematicLength);
			GUILayout.EndHorizontal();
		}
		ShowDialogue = EditorGUILayout.Foldout(ShowDialogue, "Dialogue");
		if(ShowDialogue) {
			foreach(Speach speach in Cinematic.Dialogue) {
				if(speach.TextSide == Side.Right && speach.FaceSide == Side.Right) {
					speach.FaceSide = Side.Left;
				}
				if(speach.TextSide == Side.Left && speach.FaceSide == Side.Left) {
					speach.FaceSide = Side.Right;
				}
				GUILayout.BeginHorizontal();
					if(speach.TextSide == Side.Left) {
						GUILayout.BeginVertical();
							speach.header = EditorGUILayout.TextField(speach.header);
							speach.text = EditorGUILayout.TextArea(speach.text, GUILayout.Height(80));
						GUILayout.EndVertical();
					}
					if(speach.FaceSide == Side.Left) {
						speach.face = (Texture2D)EditorGUILayout.ObjectField(speach.face, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
					}
					if(speach.TextSide == Side.Right) {
						GUILayout.BeginVertical();
							speach.header = EditorGUILayout.TextField(speach.header);
							speach.text = EditorGUILayout.TextArea(speach.text, GUILayout.Height(80));
						GUILayout.EndVertical();
					}
					if(speach.FaceSide == Side.Right) {
						speach.face = (Texture2D)EditorGUILayout.ObjectField(speach.face, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
					}
				GUILayout.EndHorizontal();
				speach.FaceSide = (Side)EditorGUILayout.EnumPopup("Face Side", speach.FaceSide);
				speach.TextSide = (Side)EditorGUILayout.EnumPopup("Text Side", speach.TextSide);
				speach.TalkSound = (AudioSource)EditorGUILayout.ObjectField("Talking Sound", speach.TalkSound, typeof(AudioSource), true);
				speach.PrintTime = EditorGUILayout.FloatField("Time Till Next Dialogue", speach.PrintTime);
				speach.shakeCamera = EditorGUILayout.Toggle("Shake When Talking", speach.shakeCamera);
			}
			GUILayout.Space(15);
			if(GUILayout.Button("Add Dialogue")) {
				Cinematic.Dialogue.Add(null);
				RemoveIndex = Cinematic.Dialogue.Count - 1;
			}
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("Remove Dialogue")) {
					Cinematic.Dialogue.RemoveAt(RemoveIndex);
					RemoveIndex--;
				}
				RemoveIndex = EditorGUILayout.IntField(RemoveIndex);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				if(GUILayout.Button("Reset Dialogue")) {
					Cinematic.Dialogue = new List<Speach>(0);
				}
			GUILayout.EndHorizontal();
		}
		ShowDebugFold = EditorGUILayout.Foldout(ShowDebugFold, "Debug Variables");
		if(ShowDebugFold) {
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.Tripped = EditorGUILayout.Toggle("Has Activated", Cinematic.Tripped);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.Space(15);
				Cinematic.PlayedAudio = EditorGUILayout.Toggle("Has Audio Played", Cinematic.PlayedAudio);
			GUILayout.EndHorizontal();
		}
	}
}