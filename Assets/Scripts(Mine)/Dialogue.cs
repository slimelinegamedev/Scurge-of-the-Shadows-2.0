using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;
using Scurge.Environment;

namespace Scurge.Environment {
	public class Dialogue : MonoBehaviour {
		public Texture2D face;
		public List<string> wordLines;
		public TextMesh pressText;
		public bool isOver = false;
		public bool speaking = false;
		public GUISkin Skin;

		void OnMouseOver() {
			pressText.gameObject.SetActive(true);
			isOver = true;
		}
		void OnMouseExit() {
			pressText.gameObject.SetActive(false);
			isOver = false;
		}

		void Update() {
			if(isOver) {
				if(Input.GetKeyDown(KeyCode.Q)) {
					Speak();
				}
			}
		}

		public void Speak() {
			speaking = true;
		}

		void OnGUI() {
			GUI.skin = Skin;

			if(speaking) {
				GUI.Box(new Rect(10, 400, 1260, 200), "");
			}
		}
	}
}