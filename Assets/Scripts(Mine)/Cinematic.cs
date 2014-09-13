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

namespace Scurge.Environment {
	public enum CinematicType {
		CircleAround,
		TopBottomCutoff,
		Tilted,
		CircleAroundTopBottomCutoff,
		TiltedCircleAround,
		TiltedTopBottomCutoff
	}
	[System.Serializable]
	public class Speach {
		public string header;
		public string text;
		public Texture2D face;
		public bool shakeCamera;
		public Side FaceSide;
		public Side TextSide = Side.Right;
		public AudioSource TalkSound;
		public float PrintTime;
	}
	public class Cinematic : MonoBehaviour {

		public Disable Disable;
		public Objects Objects;
		public CinematicType cinemaType;
		public Camera CinemaCamera;
		public Camera DarkBack;
		public bool Tripped = false;
		public bool PlayedAudio = false;
		public Scurge.Enemy.AI BossAI;
		public EnemyStats BossEnemyStats;
		public Look BossLook;
		public float BeginWaitTime;
		public float CinematicLength;
		public GUISkin Skin;
		public List<GameObject> ObjectsToActivate;
		public bool HasTripped = false;
		public AudioSource sound;
		public List<Speach> Dialogue;

		void OnGUI() {
			GUI.skin = Skin;
		}

		void Start() {
			StartCoroutine(Speak(Dialogue));
			BossEnemyStats.CanSummonMinions = false;
			CinemaCamera.gameObject.SetActive(false);
			DarkBack.gameObject.SetActive(false);
		}
		void Update() {
			if(BossEnemyStats == null && !HasTripped) {
				foreach (GameObject curObj in ObjectsToActivate) {
					curObj.SetActive(true);
				}
				sound.Play();
				HasTripped = true;
			}
		}
		IEnumerator OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				if(!Tripped) {
					yield return new WaitForSeconds(BeginWaitTime);
					StartCoroutine(Activate(cinemaType));
					Tripped = true;
				}
			}
		}
		public IEnumerator Activate(CinematicType type) {
			BossLook.camera = CinemaCamera;
			Disable.DisableObj(true, true);
			BossAI.enabled = true;
			BossEnemyStats.enabled = true;
			if(type == CinematicType.TopBottomCutoff) {
				DarkBack.gameObject.SetActive(true);
				CinemaCamera.gameObject.SetActive(true);
				CinemaCamera.rect = new Rect(0, 0.1f, 1, 0.8f);
			}
			yield return new WaitForSeconds(CinematicLength);
			BossEnemyStats.CanSummonMinions = true;
			DarkBack.gameObject.SetActive(false);
			CinemaCamera.gameObject.SetActive(false);
			Disable.EnableObj(true, true);
			BossLook.camera = Objects.Camera.GetComponent<Camera>();
		}
		public IEnumerator Speak(List<Speach> dialogue) {
			yield return new WaitForSeconds(0);
		}
	}
}