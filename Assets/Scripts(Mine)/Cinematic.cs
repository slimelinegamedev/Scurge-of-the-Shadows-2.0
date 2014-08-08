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
	public class Cinematic : MonoBehaviour {

		public Disable Disable;
		public CinematicType cinemaType;
		public Camera CinemaCamera;
		public Camera DarkBack;
		public int BeginWaitTime;
		public int CinematicLength;

		void Start() {
			CinemaCamera.gameObject.SetActive(false);
			DarkBack.gameObject.SetActive(false);
		}
		IEnumerator OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				yield return new WaitForSeconds(BeginWaitTime);
				StartCoroutine(Activate(cinemaType));
			}
		}
		public IEnumerator Activate(CinematicType type) {
			Disable.DisableObj(true, true);
			if(type == CinematicType.TopBottomCutoff) {
				DarkBack.gameObject.SetActive(true);
				CinemaCamera.gameObject.SetActive(true);
				CinemaCamera.rect = new Rect(0, 0.1f, 1, 0.8f);
			}
			yield return new WaitForSeconds(0);
		}
	}
}